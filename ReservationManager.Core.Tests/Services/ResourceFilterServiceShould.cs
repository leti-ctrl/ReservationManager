using FluentAssertions;
using NSubstitute;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Services;
using ReservationManager.DomainModel.Operation;
using FluentValidation.Results;
using Tests.EntityGenerators;

namespace Tests.Services
{
    [Trait("Category", "Unit")]
    public class ResourceFilterServiceShould
    {
        private readonly IResourceFilterService _sut;
        
        private readonly IResourceFilterDtoValidator _mockResourceFilterValidator;
        private readonly IResourceRepository _mockResourceRepository;
        private readonly IReservationRepository _mockReservationRepository;
        private readonly IClosingCalendarFilterService _mockClosingCalendarFilterService;
        
        private readonly ResourceGenerator _generator;


        public ResourceFilterServiceShould()
        {
            _mockResourceFilterValidator = Substitute.For<IResourceFilterDtoValidator>();
            _mockResourceRepository = Substitute.For<IResourceRepository>();
            _mockReservationRepository = Substitute.For<IReservationRepository>();
            _mockClosingCalendarFilterService = Substitute.For<IClosingCalendarFilterService>();
            
            _sut = new ResourceFilterService(
                _mockClosingCalendarFilterService,
                _mockResourceFilterValidator,
                Substitute.For<IResourceReservedMapper>(),
                _mockResourceRepository,
                _mockReservationRepository
            );
            
            _generator = new ResourceGenerator();
        }

        [Fact]
        public async Task ThrowArgumentException_WhenFiltersAreInvalid()
        {
            var invalidFilter = _generator.GenerateInvalidFilter();
            var validationResult = new ValidationResult(new[]
                {
                    new ValidationFailure("Day", "Invalid Day filter")
                });
            _mockResourceFilterValidator.ValidateAsync(invalidFilter)
                .Returns(Task.FromResult(validationResult));

            
            var act = async () => await _sut.GetFilteredResources(invalidFilter);

            
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid Day filter");
        }

        [Fact]
        public async Task ReturnEmpty_WhenNoResourcesFound()
        {
            var validFilter = _generator.GenerateValidFilter();
            _mockResourceFilterValidator.ValidateAsync(validFilter)
                .Returns(Task.FromResult(new ValidationResult()));
            _mockResourceRepository.GetFiltered(validFilter.TypeId, validFilter.ResourceId)
                .Returns(new List<Resource>());

            var result = await _sut.GetFilteredResources(validFilter);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnFilteredResources_WhenValidFiltersAndResourcesFound()
        {
            var validFilter = _generator.GenerateValidFilter();
            _mockResourceFilterValidator.ValidateAsync(validFilter)
                                        .Returns(Task.FromResult(new ValidationResult()));
            var resourceList = new ResourceGenerator().GenerateResourceList(5, 1);
            _mockResourceRepository.GetFiltered(validFilter.TypeId, validFilter.ResourceId)
                                   .Returns(resourceList);

            var result = await _sut.GetFilteredResources(validFilter);
            
            result.Should().NotBeEmpty();
            result.First().Description.Should().Be("Test Resource 1");
        }

        [Fact]
        public async Task ApplyClosingCalendarAndReservationFilters_WhenDateFiltersAreApplied()
        {
            var validFilter = _generator.GenerateValidFilter();
            _mockResourceFilterValidator.ValidateAsync(validFilter)
                .Returns(Task.FromResult(new ValidationResult()));
            var resourceList = new ResourceGenerator().GenerateResourceList(5, 1);
            _mockResourceRepository.GetFiltered(validFilter.TypeId, validFilter.ResourceId)
                .Returns(resourceList);
            _mockClosingCalendarFilterService.GetFiltered(Arg.Any<ClosingCalendarFilterDto>())
                .Returns(new List<ClosingCalendarDto>());
            _mockReservationRepository.GetReservationByResourceDateTimeAsync(Arg.Any<List<int>>(), Arg.Any<DateOnly>(), Arg.Any<TimeOnly>(), Arg.Any<TimeOnly>())
                .Returns(new List<Reservation>());

            
            var result = await _sut.GetFilteredResources(validFilter);

            
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ReturnEmpty_WhenNoResourcesMatchAfterFiltering()
        {
            var validFilter = _generator.GenerateValidFilter();
            _mockResourceFilterValidator.ValidateAsync(validFilter)
                .Returns(Task.FromResult(new ValidationResult()));
            
            var resourceList = new ResourceGenerator().GenerateResourceList(5, 1);
            _mockResourceRepository.GetFiltered(validFilter.TypeId, validFilter.ResourceId).Returns(resourceList);
            _mockClosingCalendarFilterService.GetFiltered(Arg.Any<ClosingCalendarFilterDto>())
                .Returns(new List<ClosingCalendarDto>());
            _mockReservationRepository.GetReservationByResourceDateTimeAsync(Arg.Any<List<int>>(), Arg.Any<DateOnly>(), Arg.Any<TimeOnly>(), Arg.Any<TimeOnly>())
                .Returns(new List<Reservation> { new Reservation() });

            
            var result = await _sut.GetFilteredResources(validFilter);

            
            result.Should().BeEmpty();
        }
    }
}
