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
        var calendars = _generator.GenerateList();
        _mockRepository.GetAllFromToday().Returns(calendars);

        var result = await _sut.GetAllFromToday();

        result.Should().NotBeNull();
        result.Should().HaveCount(calendars.Count);
        result.Should().BeEquivalentTo(calendars.Adapt<IEnumerable<ClosingCalendarDto>>());
    }

    [Fact]
    public async Task CreatesNewClosingCalendar_WhenValidInputIsProvided()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        var dto = model.Adapt<ClosingCalendarDto>();

        _mockResourceValidator.ExistingResouceId(dto.ResourceId).Returns(true);
        _mockValidator.ValidateIfAlreadyExistsClosingCalendar(dto, null).Returns(false);

        _mockRepository.CreateEntityAsync(Arg.Is<ClosingCalendar>(x =>
            x.ResourceId == model.ResourceId && x.Day == model.Day
        )).Returns(model);

        var result = await _sut.Create(dto);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dto, options => options.Excluding(x => x.Id));

        await _mockRepository.Received(1).CreateEntityAsync(Arg.Is<ClosingCalendar>(x =>
            x.ResourceId == dto.ResourceId && x.Day == dto.Day));
    }

    [Fact]
    public async Task ThrowsException_WhenResourceDoesNotExist_OnCreate()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        var dto = model.Adapt<ClosingCalendarDto>();

        _mockResourceValidator.ExistingResouceId(dto.ResourceId).Returns(false);

        var act = async () => await _sut.Create(dto);

        await act.Should().ThrowAsync<CreateNotPermittedException>()
            .WithMessage("Resource id does not exists.");

        await _mockRepository.DidNotReceive().CreateEntityAsync(Arg.Any<ClosingCalendar>());
    }

    [Fact]
    public async Task ThrowsException_WhenClosingCalendarAlreadyExists_OnCreate()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        var dto = model.Adapt<ClosingCalendarDto>();

        _mockResourceValidator.ExistingResouceId(dto.ResourceId).Returns(true);
        _mockValidator.ValidateIfAlreadyExistsClosingCalendar(dto, null).Returns(true);

        var act = async () => await _sut.Create(dto);

        await act.Should().ThrowAsync<CreateNotPermittedException>()
            .WithMessage("This closing calendar is already exists.");

        await _mockRepository.DidNotReceive().CreateEntityAsync(Arg.Any<ClosingCalendar>());
    }

    [Fact]
    public async Task DeletesClosingCalendar_WhenValidIdIsProvided()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        _mockRepository.GetEntityByIdAsync(model.Id).Returns(model);

        await _sut.Delete(model.Id);

        await _mockRepository.Received(1).DeleteEntityAsync(model);
    }

    [Fact]
    public async Task ThrowsException_WhenClosingCalendarDoesNotExist_OnDelete()
    {
        _mockRepository.GetEntityByIdAsync(Arg.Any<int>()).Returns((ClosingCalendar?)null);

        var act = async () => await _sut.Delete(1);

        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("Closing Calendar 1 not found.");

        await _mockRepository.DidNotReceive().DeleteEntityAsync(Arg.Any<ClosingCalendar>());
    }

    [Fact]
    public async Task GetsFilteredClosingCalendars_WhenValidFilterIsProvided()
    {
        var filter = new ClosingCalendarFilterDto { StartDay = DateOnly.FromDateTime(DateTime.Now) };
        var filteredResults = _generator.GenerateList().Adapt<List<ClosingCalendarDto>>();

        _mockFilterService.GetFiltered(filter).Returns(filteredResults);

        var result = await _sut.GetFiltered(filter);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(filteredResults);

        await _mockFilterService.Received(1).GetFiltered(filter);
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

        _mockResourceValidator.ExistingResouceId(dto.ResourceId).Returns(true);
        _mockValidator.ValidateIfAlreadyExistsClosingCalendar(dto, model.Id).Returns(false);

        _mockRepository.UpdateEntityAsync(Arg.Is<ClosingCalendar>(x =>
            x.Id == model.Id && x.ResourceId == model.ResourceId && x.Day == model.Day
        )).Returns(updatedModel);

        var result = await _sut.Update(model.Id, dto);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedModel.Adapt<ClosingCalendarDto>());

        await _mockRepository.Received(1).UpdateEntityAsync(Arg.Any<ClosingCalendar>());
    }

    [Fact]
    public async Task ThrowsException_WhenResourceDoesNotExist_OnUpdate()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        var dto = model.Adapt<ClosingCalendarDto>();

        _mockResourceValidator.ExistingResouceId(dto.ResourceId).Returns(false);

        var act = async () => await _sut.Update(model.Id, dto);

        await act.Should().ThrowAsync<CreateNotPermittedException>()
            .WithMessage("Resource id does not exists.");

        await _mockRepository.DidNotReceive().UpdateEntityAsync(Arg.Any<ClosingCalendar>());
    }

    [Fact]
    public async Task ThrowsException_WhenClosingCalendarAlreadyExists_OnUpdate()
    {
        var model = _generator.GenerateSingleClosingCalendar();
        var dto = model.Adapt<ClosingCalendarDto>();

        _mockResourceValidator.ExistingResouceId(dto.ResourceId).Returns(true);
        _mockValidator.ValidateIfAlreadyExistsClosingCalendar(dto, model.Id).Returns(true);

        var act = async () => await _sut.Update(model.Id, dto);

        await act.Should().ThrowAsync<CreateNotPermittedException>()
            .WithMessage("This closing calendar is already exists.");

        await _mockRepository.DidNotReceive().UpdateEntityAsync(Arg.Any<ClosingCalendar>());
    }
}