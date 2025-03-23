using FluentAssertions;
using Mapster;
using NSubstitute;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Services;
using ReservationManager.DomainModel.Operation;
using Tests.EntityGenerators;

namespace Tests.Services;

[Trait("Category", "Unit")]
public class ClosingCalendarServiceShould
{
    private readonly IClosingCalendarService _sut;
    
    private readonly IClosingCalendarRepository _mockClosingCalendarRepository;
    private readonly IResourceValidator _mockResourceValidator;
    private readonly IClosingCalendarValidator _mockClosingCalendarValidator;
    private readonly IClosingCalendarFilterService _mockClosingCalendarFilterService;
    private readonly IResourceService _mockResourceService;
    
    public ClosingCalendarServiceShould()
    {
        _mockClosingCalendarRepository = Substitute.For<IClosingCalendarRepository>();
        _mockResourceValidator = Substitute.For<IResourceValidator>();
        _mockClosingCalendarValidator = Substitute.For<IClosingCalendarValidator>();
        _mockClosingCalendarFilterService = Substitute.For<IClosingCalendarFilterService>();
        _mockResourceService = Substitute.For<IResourceService>();

        _sut = new ClosingCalendarService(
            _mockClosingCalendarRepository,
            _mockClosingCalendarFilterService,
            _mockResourceValidator,
            _mockClosingCalendarValidator,
            _mockResourceService
        );
        
    }

    [Fact]
    public async Task ReturnListOfClosingCalendarDto_GetAllFromToday()
    {
        var day = DateOnly.FromDateTime(DateTime.Now);
        var day2 = day.AddDays(1);
        var closingCalendar = new List<ClosingCalendar>
        {
            new ClosingCalendar { Day = day },
            new ClosingCalendar { Day = day2 }
        };
        var closingCalendarDto = new List<ClosingCalendarDto>
        {
            new ClosingCalendarDto { Day = day },
            new ClosingCalendarDto { Day = day2 }
        };
        _mockClosingCalendarRepository.GetAllFromToday().Returns(closingCalendar);

        var result = await _sut.GetAllFromToday();

        result.Should().NotBeNull();
        result.Should().HaveCount(closingCalendar.Count);
        result.Should().BeEquivalentTo(closingCalendarDto);
    }
    
    [Fact]
    public async Task ReturnFilteredClosingCalendars_WhenValidFilterIsProvided()
    {
        var day = DateOnly.FromDateTime(DateTime.Now);
        var filter = new ClosingCalendarFilterDto { StartDay =  day };
        var filteredResults = new List<ClosingCalendarDto> { new ClosingCalendarDto { Day = day } };
        _mockClosingCalendarFilterService.GetFiltered(Arg.Any<ClosingCalendarFilterDto>())
                                         .Returns(filteredResults);

        var result = await _sut.GetFiltered(filter);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(filteredResults);
    }
    
    [Fact]
    public async Task ThrowsCreateNotPermittedException_Create_WhenResourceDoesNotExist()
    {
        var resourceId = -1;
        var day = new DateOnly(2020, 01, 01);
        var description = "test";
        var dto = new ClosingCalendarDto { ResourceId = resourceId, Day = day, Description =  description};
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>())
                              .Returns(false);

        var act = async () => await _sut.Create(dto);

        await act.Should()
                 .ThrowAsync<CreateNotPermittedException>()
                 .WithMessage("Resource id does not exists.");
    }
    
    [Fact]
    public async Task ThrowsCreateNotPermittedException_Create_WhenClosingCalendarAlreadyExists()
    {
        var resourceId = 1;
        var day = new DateOnly(2020, 01, 01);
        var description = "test";
        var dto = new ClosingCalendarDto { ResourceId = resourceId, Day = day, Description =  description};
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>())
                              .Returns(true);
        _mockClosingCalendarValidator.ExistingClosignCalendar(Arg.Any<int>(), Arg.Any<DateOnly>(), Arg.Any<int?>())
                      .Returns(true);

        var act = async () => await _sut.Create(dto);

        await act.Should()
                 .ThrowAsync<CreateNotPermittedException>()
                 .WithMessage("This closing calendar is already exists.");
    }

    [Fact]
    public async Task ReturnNewClosingCalendarDto_Create_WhenValidClosingCalendarDtoIsProvided()
    {
        var resourceId = 1;
        var description = "test";
        var closingCalendarId = 42;
        var day = DateOnly.FromDateTime(DateTime.Now);
        var entityCreated = new ClosingCalendar
        {
            Id = closingCalendarId, 
            ResourceId = resourceId, 
            Day = day, 
            Description = description
        };
        var dto = new ClosingCalendarDto { ResourceId = resourceId, Day = day, Description = description };
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>())
                              .Returns(true);
        _mockClosingCalendarValidator.ExistingClosignCalendar(Arg.Any<int>(), Arg.Any<DateOnly>(), Arg.Any<int?>())
                      .Returns(false);
        _mockClosingCalendarRepository.CreateEntityAsync(Arg.Any<ClosingCalendar>(), Arg.Any<CancellationToken>())
                       .Returns(entityCreated);

        var result = await _sut.Create(dto);

        result.Should().NotBeNull();
        result.Id.Should().Be(closingCalendarId);
        result.ResourceId.Should().Be(resourceId);
        result.Day.Should().Be(day);
        result.Description.Should().Be(description);
    }
    
    [Fact]
    public async Task ThrowsCreateNotPermittedException_BulkCreate_WhenResourceTypeDoesNotExist()
    {
        var resourceTypeId = -1;
        var from = new DateOnly(2020, 01, 01);
        var to = new DateOnly(2020, 01, 10);
        var description = "test";
        var dto = new BulkClosingCalendarDto
        {
            ResourceTypeId = resourceTypeId, 
            From = from, 
            To =  to, 
            Description =  description
        };
        _mockResourceValidator.ValidateResourceType(Arg.Any<int>())
            .Returns(false);

        var result = async () => await _sut.BulkCreate(dto);

        await result.Should()
            .ThrowAsync<CreateNotPermittedException>()
            .WithMessage($"Resource type {resourceTypeId} does not exist.");
    }
    
    [Fact]
    public async Task ReturnEmptyList_BulkCreate_WhenResourcesIsEmpty()
    {
        var resourceTypeId = 1;
        var from = new DateOnly(2020, 01, 01);
        var to = new DateOnly(2020, 01, 10);
        var description = "test";
        var dto = new BulkClosingCalendarDto
        {
            ResourceTypeId = resourceTypeId, 
            From = from, 
            To =  to, 
            Description =  description
        };
        var resourceFilter = new ResourceFilterDto { TypeId = resourceTypeId };
        _mockResourceValidator.ValidateResourceType(Arg.Any<int>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(Enumerable.Empty<ResourceDto>().ToList());
        
        var result = await _sut.BulkCreate(dto);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnEmptyList_BulkCreate_WhenExistingAllClosingCalendars()
    {
        var resourceTypeId = 1;
        var resourceTypeCode = "test";
        var resourceId = 42;
        var from = new DateOnly(2020, 01, 01);
        var to = new DateOnly(2020, 01, 02);
        var description = "test";
        var dto = new BulkClosingCalendarDto
        {
            ResourceTypeId = resourceTypeId, 
            From = from, 
            To =  to, 
            Description =  description
        };
        var resourceList = new List<ResourceDto>()
        {
            {
                new ResourceDto() { 
                    Description = "", 
                    Id = resourceId, 
                    Type = new ResourceTypeDto
                    {
                        Id = resourceTypeId, 
                        Code = resourceTypeCode
                    }
                }
            }
        };
        var closingCalendar = new List<ClosingCalendar>
        {
            new ClosingCalendar { Day = from, Description = description, ResourceId = resourceId },
            new ClosingCalendar { Day = to, Description = description, ResourceId = resourceId },
        };
        var resourceFilter = new ResourceFilterDto { TypeId = resourceTypeId };
        _mockResourceValidator.ValidateResourceType(Arg.Any<int>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(resourceFilter)
            .Returns(resourceList);
        _mockClosingCalendarRepository.GetExistingClosingCalendars(new List<int> { resourceId }, new List<DateOnly> { from, to })
            .Returns(closingCalendar);
        
        var result = await _sut.BulkCreate(dto);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task NotCreateDuplicateClosingCalendarEntries_BulkCreate_WhenNotExistingClosingCalendars()
    {
        var resourceTypeId = 1;
        var resourceTypeCode = "test";
        var resourceId = 42;
        var from = new DateOnly(2020, 01, 01);
        var to = new DateOnly(2020, 01, 02);
        var description = "test";
        var dto = new BulkClosingCalendarDto
        {
            ResourceTypeId = resourceTypeId, 
            From = from, 
            To =  to, 
            Description =  description
        };
        var resourceList = new List<ResourceDto>()
        {
            {
                new ResourceDto() { 
                    Description = "", 
                    Id = resourceId, 
                    Type = new ResourceTypeDto
                    {
                        Id = resourceTypeId, 
                        Code = resourceTypeCode
                    }
                }
            }
        };
        var closingCalendar = new List<ClosingCalendar>
        {
            new ClosingCalendar { Day = from, Description = description, ResourceId = resourceId },
            new ClosingCalendar { Day = to, Description = description, ResourceId = resourceId },
        };
        _mockResourceValidator.ValidateResourceType(Arg.Any<int>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(resourceList);
        _mockClosingCalendarRepository.GetExistingClosingCalendars(Arg.Any<List<int>>(), Arg.Any<List<DateOnly>>())
            .Returns(Enumerable.Empty<ClosingCalendar>().ToList());
        _mockClosingCalendarRepository.CreateEntitiesAsync(Arg.Any<List<ClosingCalendar>>())
            .Returns(closingCalendar);
        
        var result = await _sut.BulkCreate(dto);

        result.Should().NotBeEmpty();
        result.Count().Should().Be(closingCalendar.Count);
        result.First().Day.Should().Be(from);
        result.First().Description.Should().Be(description);
        result.First().ResourceId.Should().Be(resourceId);
        result.Last().Description.Should().Be(description);
        result.Last().ResourceId.Should().Be(resourceId);
        result.Last().Day.Should().Be(to);
    }

    [Fact]
public async Task ReturnClosingCalendar_BulkCreate_WhenNotExistingClosingCalendars()
{
    // Arrange
    var resourceTypeId = 1;
    var resourceTypeCode = "test";
    var resourceId = 42;
    var from = new DateOnly(2020, 01, 01);
    var to = new DateOnly(2020, 01, 02);
    var description = "test";
    
    var dto = new BulkClosingCalendarDto
    {
        ResourceTypeId = resourceTypeId, 
        From = from, 
        To = to, 
        Description = description
    };

    var resourceList = new List<ResourceDto>
    {
        new ResourceDto
        { 
            Description = "", 
            Id = resourceId, 
            Type = new ResourceTypeDto
            {
                Id = resourceTypeId, 
                Code = resourceTypeCode
            }
        }
    };

    var expectedClosingCalendars = new List<ClosingCalendar>
    {
        new ClosingCalendar { Day = from, Description = description, ResourceId = resourceId },
        new ClosingCalendar { Day = to, Description = description, ResourceId = resourceId }
    };

    _mockResourceValidator.ValidateResourceType(resourceTypeId)
        .Returns(true);

    _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
        .Returns(resourceList);

    _mockClosingCalendarRepository.GetExistingClosingCalendars(Arg.Any<List<int>>(), Arg.Any<List<DateOnly>>())
        .Returns(Enumerable.Empty<ClosingCalendar>().ToList()); // Nessun calendario esistente

    _mockClosingCalendarRepository.CreateEntitiesAsync(Arg.Any<List<ClosingCalendar>>())
        .Returns(expectedClosingCalendars); // Simula il ritorno delle entità create

    // Act
    var result = await _sut.BulkCreate(dto);

    // Assert
    result.Should().NotBeEmpty();
    result.Should().HaveCount(expectedClosingCalendars.Count);

    result.First().Day.Should().Be(from);
    result.First().Description.Should().Be(description);
    result.First().ResourceId.Should().Be(resourceId);

    result.Last().Day.Should().Be(to);
    result.Last().Description.Should().Be(description);
    result.Last().ResourceId.Should().Be(resourceId);

    _mockClosingCalendarRepository.Received(1).CreateEntitiesAsync(Arg.Any<List<ClosingCalendar>>());
}

    
    [Fact]
    public async Task ThrowsCreateNotPermittedException_Update_WhenResourceDoesNotExist()
    {
        var resourceId = -1;
        var closingCalendarId = -1;
        var day = new DateOnly(2020, 01, 01);
        var description = "test";
        var dto = new ClosingCalendarDto { ResourceId = resourceId, Day = day, Description =  description};
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>())
            .Returns(false);

        var result = async () => await _sut.Update(closingCalendarId, dto);

        await result.Should()
            .ThrowAsync<CreateNotPermittedException>()
            .WithMessage("Resource id does not exists.");
    }
    
    [Fact]
    public async Task ThrowsCreateNotPermittedException_Update_WhenClosingCalendarAlreadyExists()
    {
        var resourceId = 1;
        var closingCalendarId = 1;
        var day = new DateOnly(2020, 01, 01);
        var description = "test";
        var dto = new ClosingCalendarDto { ResourceId = resourceId, Day = day, Description =  description};
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>())
            .Returns(true);
        _mockClosingCalendarValidator.ExistingClosignCalendar(Arg.Any<int>(), Arg.Any<DateOnly>(), Arg.Any<int?>())
            .Returns(true);

        var result = async () => await _sut.Update(closingCalendarId, dto);

        await result.Should()
            .ThrowAsync<CreateNotPermittedException>()
            .WithMessage("This closing calendar is already exists.");
    }
    
    [Fact]
    public async Task ReturnUpdatedClosingCalendarDto_Update_WhenValidClosingCalendarDtoIsProvided()
    {
        var closingCalendarId = 1;
        var resourceId = 1;
        var description = "test";
        var day = DateOnly.FromDateTime(DateTime.Now);
        var entity = new ClosingCalendar { Id = closingCalendarId, ResourceId = resourceId, Day = day, Description = description };
        var dto = new ClosingCalendarDto { ResourceId = resourceId, Day = day, Description = description };
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>())
            .Returns(true);
        _mockClosingCalendarValidator.ExistingClosignCalendar(Arg.Any<int>(), Arg.Any<DateOnly>(), Arg.Any<int?>())
            .Returns(false);
        _mockClosingCalendarRepository.UpdateEntityAsync(Arg.Any<ClosingCalendar>())
            .Returns(entity);

        var result = await _sut.Update(closingCalendarId, dto);

        result.Should().NotBeNull();
        result.Id.Should().Be(closingCalendarId);
        result.ResourceId.Should().Be(resourceId);
        result.Day.Should().Be(day);
        result.Description.Should().Be(description);
    }

    [Fact]
    public async Task ThrowsException_Delete_WhenClosingCalendarDoesNotExist()
    {
        var entityId = 1;
        _mockClosingCalendarRepository.GetEntityByIdAsync(Arg.Any<int>()).Returns((ClosingCalendar?)null);

        var act = async () => await _sut.Delete(entityId);

        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage($"Closing Calendar {entityId} not found.");
    }
    
    [Fact]
    public async Task RemoveClosingCalendar_Delete_WhenClosingCalendarExist()
    {
        var entityId = 1;
        var entity = new ClosingCalendar { Id = entityId };
        _mockClosingCalendarRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(entity);

        var act = async () => await _sut.Delete(entityId);

        await act.Should().NotThrowAsync();
    }
}