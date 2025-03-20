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
    
    private readonly IClosingCalendarRepository _mockRepository;
    private readonly IResourceValidator _mockResourceValidator;
    private readonly IClosingCalendarValidator _mockValidator;
    private readonly IClosingCalendarFilterService _mockFilterService;
    private readonly IResourceService _mockResourceService;

    private readonly ClosingCalendarGenerator _generator;

    public ClosingCalendarServiceShould()
    {
        _mockRepository = Substitute.For<IClosingCalendarRepository>();
        _mockResourceValidator = Substitute.For<IResourceValidator>();
        _mockValidator = Substitute.For<IClosingCalendarValidator>();
        _mockFilterService = Substitute.For<IClosingCalendarFilterService>();
        _mockResourceService = Substitute.For<IResourceService>();

        _sut = new ClosingCalendarService(
            _mockRepository,
            _mockFilterService,
            _mockResourceValidator,
            _mockValidator,
            _mockResourceService
        );
        
        _generator = new ClosingCalendarGenerator();
    }

    [Fact]
    public async Task GetsAllFromToday()
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
        _mockRepository.GetAllFromToday().Returns(closingCalendar);

        var result = await _sut.GetAllFromToday();

        result.Should().NotBeNull();
        result.Should().HaveCount(closingCalendar.Count);
        result.Should().BeEquivalentTo(closingCalendarDto);
    }

    [Fact]
    public async Task CreatesNewClosingCalendar_WhenValidInputIsProvided()
    {
        var resourceId = 1;
        var day = DateOnly.FromDateTime(DateTime.Now);
        var model = new ClosingCalendar { ResourceId = resourceId, Day = day };
        var dto = new ClosingCalendarDto { ResourceId = resourceId, Day = day };
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>()).Returns(true);
        _mockValidator.ExistingClosignCalendar(Arg.Any<int>(), Arg.Any<DateOnly>(), null).Returns(false);
        _mockRepository.CreateEntityAsync(Arg.Any<ClosingCalendar>()).Returns(model);

        var result = await _sut.Create(dto);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dto, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task ThrowsException_WhenResourceDoesNotExist_OnCreate()
    {
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>()).Returns(false);

        var act = async () => await _sut.Create(Arg.Any<ClosingCalendarDto>());

        await act.Should().ThrowAsync<CreateNotPermittedException>()
            .WithMessage("Resource id does not exists.");
    }

    [Fact]
    public async Task ThrowsException_WhenClosingCalendarAlreadyExists_OnCreate()
    {
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>()).Returns(true);
        _mockValidator.ExistingClosignCalendar(Arg.Any<int>(), Arg.Any<DateOnly>(), null).Returns(true);

        var act = async () => await _sut.Create(Arg.Any<ClosingCalendarDto>());

        await act.Should().ThrowAsync<CreateNotPermittedException>()
            .WithMessage("This closing calendar is already exists.");
    }

    [Fact]
    public async Task DeletesClosingCalendar_WhenValidIdIsProvided()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        _mockRepository.GetEntityByIdAsync(Arg.Any<int>()).Returns(model);

        await _sut.Delete(model.Id);

        await _mockRepository.Received(1).DeleteEntityAsync(model);
    }

    [Fact]
    public async Task ThrowsException_WhenClosingCalendarDoesNotExist_OnDelete()
    {
        _mockRepository.GetEntityByIdAsync(Arg.Any<int>()).Returns((ClosingCalendar?)null);

        var act = async () => await _sut.Delete(Arg.Any<int>());

        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("Closing Calendar 1 not found.");
    }

    [Fact]
    public async Task GetsFilteredClosingCalendars_WhenValidFilterIsProvided()
    {
        var day = DateOnly.FromDateTime(DateTime.Now);
        var filter = new ClosingCalendarFilterDto { StartDay =  day };
        var filteredResults = new List<ClosingCalendarDto> { new ClosingCalendarDto { Day = day } };
        _mockFilterService.GetFiltered(Arg.Any<ClosingCalendarFilterDto>()).Returns(filteredResults);

        var result = await _sut.GetFiltered(filter);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(filteredResults);
    }

    [Fact]
    public async Task BulkCreatesClosingCalendars_WhenValidInputIsProvided()
    {
        // Arrange
        var bulkDto = new BulkClosingCalendarDto
        {
            ResourceTypeId = 1,
            From = DateOnly.FromDateTime(DateTime.Now),
            To = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
            Description = "Test Bulk"
        };

        var resources = new List<ResourceDto>
        {
            new ResourceDto
            {
                Id = 1,
                Description = ""
            },
            new ResourceDto
            {
                Id = 2,
                Description = ""
            }
        };

        var newClosingCalendars = _generator.GenerateList();

        _mockResourceValidator.ValidateResourceType(bulkDto.ResourceTypeId).Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>()).Returns(resources);

        _mockRepository.GetExistingClosingCalendars(Arg.Any<IEnumerable<int>>(), Arg.Any<IEnumerable<DateOnly>>())
            .Returns(new List<ClosingCalendar>());

        _mockRepository.CreateEntitiesAsync(Arg.Any<List<ClosingCalendar>>())
            .Returns(newClosingCalendars);

        // Act
        var result = await _sut.BulkCreate(bulkDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(newClosingCalendars.Count);
        result.Should().BeEquivalentTo(newClosingCalendars.Adapt<IEnumerable<ClosingCalendarDto>>());

        await _mockRepository.Received(1).CreateEntitiesAsync(Arg.Any<List<ClosingCalendar>>());
    }

    [Fact]
    public async Task ThrowsException_WhenResourceTypeDoesNotExist_OnBulkCreate()
    {
        var bulkDto = new BulkClosingCalendarDto { ResourceTypeId = 999 };

        _mockResourceValidator.ValidateResourceType(bulkDto.ResourceTypeId).Returns(false);

        // Act
        var act = async () => await _sut.BulkCreate(bulkDto);

        // Assert
        await act.Should().ThrowAsync<CreateNotPermittedException>()
            .WithMessage($"Resource type {bulkDto.ResourceTypeId} does not exist.");

        await _mockRepository.DidNotReceive().CreateEntitiesAsync(Arg.Any<List<ClosingCalendar>>());
    }

    [Fact]
    public async Task UpdatesClosingCalendar_WhenValidInputIsProvided()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        var dto = model.Adapt<ClosingCalendarDto>();
        var updatedModel = model;
        updatedModel.Description = "Updated Description";

        _mockResourceValidator.ExistingResouceId(Arg.Any<int>())
            .Returns(true);
        _mockValidator.ExistingClosignCalendar(Arg.Any<int>(), Arg.Any<DateOnly>(), Arg.Any<int?>())
            .Returns(false);
        _mockRepository.UpdateEntityAsync(Arg.Any<ClosingCalendar>())
            .Returns(updatedModel);

        var result = await _sut.Update(model.Id, dto);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedModel.Adapt<ClosingCalendarDto>());

        //TODO rimuovere
        await _mockRepository.Received(1).UpdateEntityAsync(Arg.Any<ClosingCalendar>());
    }

    [Fact]
    public async Task ThrowsException_WhenResourceDoesNotExist_OnUpdate()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        var dto = model.Adapt<ClosingCalendarDto>();
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>()).Returns(false);

        var act = async () => await _sut.Update(model.Id, dto);

        await act.Should().ThrowAsync<CreateNotPermittedException>()
            .WithMessage("Resource id does not exists.");

        //TODO rimuovere
        await _mockRepository.DidNotReceive().UpdateEntityAsync(Arg.Any<ClosingCalendar>());
    }

    [Fact]
    public async Task ThrowsException_WhenClosingCalendarAlreadyExists_OnUpdate()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        _mockResourceValidator.ExistingResouceId(Arg.Any<int>())
                              .Returns(true);
        _mockValidator.ExistingClosignCalendar(Arg.Any<int>(), Arg.Any<DateOnly>(), Arg.Any<int?>())
                      .Returns(true);

        var act = async () => await _sut.Update(model.Id, model.Adapt<ClosingCalendarDto>());

        await act.Should().ThrowAsync<CreateNotPermittedException>()
                 .WithMessage("This closing calendar is already exists.");
        //TODO rimuovere
        await _mockRepository.DidNotReceive().UpdateEntityAsync(Arg.Any<ClosingCalendar>());
    }
}