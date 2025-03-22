using System.ComponentModel.DataAnnotations;
using FluentAssertions;
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
public class ClosingCalendarFilterServiceShould
{
    private readonly IClosingCalendarFilterService _sut;
    
    private readonly IClosingCalendarFilterDtoValidator _mockDtoValidator;
    private readonly IClosingCalendarRepository _mockRepository;
    
    private readonly ClosingCalendarFilterGenerator _generator;


    public ClosingCalendarFilterServiceShould()
    {
        _mockDtoValidator = Substitute.For<IClosingCalendarFilterDtoValidator>();
        _mockRepository = Substitute.For<IClosingCalendarRepository>();
        
        _sut = new ClosingCalendarFilterService(_mockDtoValidator, _mockRepository);
        
        _generator = new ClosingCalendarFilterGenerator(); 
    }

    [Fact]
    public async Task ThrowsInvalidFiltersException_WhenInvalidDateRangePassed()
    {
        var filter = _generator.GenerateInvalidFilter();
        _mockDtoValidator.Validate(filter)
            .Returns(new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("StartDay", "StartDay must be before or equal to EndDay.")
            }));


        var act = async () => await _sut.GetFiltered(filter);

        await act.Should().ThrowAsync<InvalidFiltersException>()
                 .WithMessage("You cannot set end date without a start date.");
    }

    [Fact]
    public async Task ReturnsEmptyList_WhenRepositoryReturnsNoResults()
    {
        var filter = _generator.GenerateValidFilter();
        _mockDtoValidator.Validate(filter).Returns(new FluentValidation.Results.ValidationResult());
        _mockRepository.GetFiltered(null, 
                            filter.StartDay, 
                            filter.EndDay, 
                            null, 
                            null)
            .Returns(Enumerable.Empty<ClosingCalendar>());

        var result = await _sut.GetFiltered(filter);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnsFilteredAndOrderedResults_WhenRepositoryReturnsData()
    {
        var filter = _generator.GenerateValidFilter();
        var dataFromRepo = new ClosingCalendarGenerator().GenerateList();
        _mockDtoValidator.Validate(filter).Returns(new FluentValidation.Results.ValidationResult());
        _mockRepository.GetFiltered(null, 
                filter.StartDay, 
                filter.EndDay, 
                null, 
                null)
            .Returns(dataFromRepo);

        var result = await _sut.GetFiltered(filter);

        result.Should().NotBeNull();
        result.Should().HaveCount(dataFromRepo.Count);
        result.Should().BeInAscendingOrder(r => r.Day);
    }

    [Fact]
    public async Task CallsRepositoryWithCorrectParameters_WhenValidFilterIsPassed()
    {
        var filter = _generator.GenerateFilter();
        _mockDtoValidator.Validate(filter).Returns(new FluentValidation.Results.ValidationResult());

        await _sut.GetFiltered(filter);

        await _mockRepository.Received(1).GetFiltered(filter.Id, filter.StartDay, filter.EndDay, filter.RescourceId, filter.ResourceTypeId);
    }
}
