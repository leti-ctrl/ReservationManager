using FluentAssertions;
using NSubstitute;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Services;
using ReservationManager.DomainModel.Meta;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Operation;
using Tests.EntityGenerators;

namespace Tests.Services
{
    [Trait("Category", "Unit")]
    public class ReservationTypeServiceShould
    {
        private readonly IReservationTypeService _sut;
        
        private readonly IReservationTypeRepository _mockReservationTypeRepository;
        private readonly IReservationRepository _mockReservationRepository;
        private readonly IReservationTypeValidator _mockValidator;
        
        private readonly ReservationTypeServiceGenerator _generator;

        public ReservationTypeServiceShould()
        {
            _mockReservationTypeRepository = Substitute.For<IReservationTypeRepository>();
            _mockReservationRepository = Substitute.For<IReservationRepository>();
            _mockValidator = Substitute.For<IReservationTypeValidator>();
            _sut = new ReservationTypeService(
                _mockReservationTypeRepository,
                _mockReservationRepository,
                _mockValidator
            );
            _generator = new ReservationTypeServiceGenerator();
        }

        [Fact]
        public async Task GetAllReservationType_ReturnsSortedReservationTypes()
        {
            var types = _generator.GenerateReservationTypeList();
            _mockReservationTypeRepository.GetAllTypesAsync().Returns(types);

            var result = await _sut.GetAllReservationType();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.First().Code.Should().Be("A");
        }

        [Fact]
        public async Task CreateReservationType_ReturnsCreatedDto_WhenValidRequest()
        {
            var request = _generator.GenerateUpsertDto("A", "Test", new TimeOnly(8, 0), new TimeOnly(10, 0));
            var newType = _generator.GenerateReservationType(1, "A", "Test", new TimeOnly(8, 0), new TimeOnly(10, 0));

            _mockValidator.ValidateAsync(Arg.Any<ReservationType>()).Returns(new FluentValidation.Results.ValidationResult());
            _mockReservationTypeRepository.CreateTypeAsync(Arg.Any<ReservationType>()).Returns(newType);

            var result = await _sut.CreateReservationType(request);

            result.Should().NotBeNull();
            result.Code.Should().Be("A");
            result.Name.Should().Be("Test");
        }

        [Fact]
        public async Task CreateReservationType_ThrowsInvalidCodeTypeException_WhenCodeExists()
        {
            var request = _generator.GenerateUpsertDto("A", "Test", new TimeOnly(8, 0), new TimeOnly(10, 0));
            var existingType = _generator.GenerateReservationType(2, "A", "Duplicate", new TimeOnly(9, 0), new TimeOnly(11, 0));

            _mockValidator.ValidateAsync(Arg.Any<ReservationType>()).Returns(new FluentValidation.Results.ValidationResult());
            _mockReservationTypeRepository.GetByCodeAsync("A").Returns(existingType);

            var act = async () => await _sut.CreateReservationType(request);

            await act.Should().ThrowAsync<InvalidCodeTypeException>()
                .WithMessage("Reservation type with code A already exists");
        }

        [Fact]
        public async Task UpdateReservationType_ReturnsUpdatedDto_WhenValidRequest()
        {
            var id = 1;
            var request = _generator.GenerateUpsertDto("A", "Updated", new TimeOnly(9, 0), new TimeOnly(11, 0));
            var existingType = _generator.GenerateReservationType(1, "A", "Old Name", new TimeOnly(8, 0), new TimeOnly(10, 0));
            var updatedType = _generator.GenerateReservationType(1, "A", "Updated", new TimeOnly(9, 0), new TimeOnly(11, 0));

            _mockReservationTypeRepository.GetTypeById(id).Returns(existingType);
            _mockValidator.ValidateAsync(Arg.Any<ReservationType>()).Returns(new FluentValidation.Results.ValidationResult());
            _mockReservationTypeRepository.UpdateTypeAsync(Arg.Any<ReservationType>()).Returns(updatedType);

            var result = await _sut.UpdateReservationType(id, request);

            result.Should().NotBeNull();
            result.Name.Should().Be("Updated");
        }

        [Fact]
        public async Task DeleteReservationType_ThrowsDeleteNotPermittedException_WhenFutureReservationsExist()
        {
            var id = 1;
            var type = _generator.GenerateReservationType(1, "A", "Type A", new TimeOnly(10, 0), new TimeOnly(12, 0));

            _mockReservationTypeRepository.GetTypeById(id)
                .Returns(type);
            _mockReservationRepository.GetReservationByTypeIdAfterTodayAsync(id)
                .Returns(new List<Reservation> { new Reservation() });

            var act = async () => await _sut.DeleteReservationType(id);

            await act.Should().ThrowAsync<DeleteNotPermittedException>()
                .WithMessage("Cannot delete A because exits future reservations with this type");
        }

        [Fact]
        public async Task DeleteReservationType_DeletesSuccessfully_WhenNoFutureReservationsExist()
        {
            var id = 1;
            var type = _generator.GenerateReservationType(1, "A", "Type A", new TimeOnly(10, 0), new TimeOnly(12, 0));
            _mockReservationTypeRepository.GetTypeById(id)
                .Returns(type);
            _mockReservationRepository.GetReservationByTypeIdAfterTodayAsync(id)
                .Returns(Enumerable.Empty<Reservation>());

            await _sut.DeleteReservationType(id);

            await _mockReservationTypeRepository.Received(1).DeleteTypeAsync(type);
        }
    }
}
