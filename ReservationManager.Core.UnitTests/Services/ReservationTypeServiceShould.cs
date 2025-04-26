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
        private readonly ReservationTypeService _sut;
        
        private readonly IReservationTypeRepository _mockReservationTypeRepository;
        private readonly IReservationRepository _mockReservationRepository;
        private readonly IReservationTypeValidator _mockValidator;
        
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
        }

        [Fact]
        public async Task GetAllReservationType_ReturnsSortedReservationTypes()
        {
            var start = new TimeOnly(10, 0);
            var end = new TimeOnly(20, 30);
            var types = new List<ReservationType>
            {
                ReservationTypeServiceGenerator.GenerateReservationType(1, "A", "Type A", start, end),
                ReservationTypeServiceGenerator.GenerateReservationType(2, "B", "Type B", start, end)
            };
            _mockReservationTypeRepository.GetAllTypesAsync().Returns(types);

            var result = await _sut.GetAllReservationType();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1);
            result.Last().Id.Should().Be(2);
            result.First().Code.Should().Be("A");
            result.Last().Code.Should().Be("B");
            result.First().Name.Should().Be("Type A");
            result.Last().Name.Should().Be("Type B");
            result.First().Start.Should().Be(start);
            result.Last().Start.Should().Be(start);
            result.First().End.Should().Be(end);
            result.Last().End.Should().Be(end);
        }

        [Fact]
        public async Task ReturnsCreatedDto_CreateReservationType_WhenValidRequest()
        {
            var request = ReservationTypeServiceGenerator.GenerateUpsertDto("A", "Test", new TimeOnly(8, 0), new TimeOnly(10, 0));
            var newType = ReservationTypeServiceGenerator.GenerateReservationType(1, "A", "Test", new TimeOnly(8, 0), new TimeOnly(10, 0));

            _mockValidator.ValidateAsync(Arg.Any<ReservationType>()).Returns(new FluentValidation.Results.ValidationResult());
            _mockReservationTypeRepository.CreateTypeAsync(Arg.Any<ReservationType>()).Returns(newType);

            var result = await _sut.CreateReservationType(request);

            result.Should().NotBeNull();
            result.Code.Should().Be("A");
            result.Name.Should().Be("Test");
        }

        [Fact]
        public async Task ThrowsInvalidCodeTypeException_CreateReservationType_WhenCodeExists()
        {
            var request = ReservationTypeServiceGenerator.GenerateUpsertDto("A", "Test", new TimeOnly(8, 0), new TimeOnly(10, 0));
            var existingType = ReservationTypeServiceGenerator.GenerateReservationType(2, "A", "Duplicate", new TimeOnly(9, 0), new TimeOnly(11, 0));

            _mockValidator.ValidateAsync(Arg.Any<ReservationType>())
                .Returns(new FluentValidation.Results.ValidationResult());
            _mockReservationTypeRepository.GetTypeByCode("A")
                .Returns(existingType);

            var act = async () => await _sut.CreateReservationType(request);

            await act.Should().ThrowAsync<InvalidCodeTypeException>()
                .WithMessage("Reservation type with code A already exists");
        }

        [Fact]
        public async Task ReturnsUpdatedDto_UpdateReservationType_WhenValidRequest()
        {
            var id = 1;
            var request = ReservationTypeServiceGenerator.GenerateUpsertDto("A", "Updated", new TimeOnly(9, 0), new TimeOnly(11, 0));
            var existingType = ReservationTypeServiceGenerator.GenerateReservationType(1, "A", "Old Name", new TimeOnly(8, 0), new TimeOnly(10, 0));
            var updatedType = ReservationTypeServiceGenerator.GenerateReservationType(1, "A", "Updated", new TimeOnly(9, 0), new TimeOnly(11, 0));

            _mockReservationTypeRepository.GetTypeById(id).Returns(existingType);
            _mockValidator.ValidateAsync(Arg.Any<ReservationType>()).Returns(new FluentValidation.Results.ValidationResult());
            _mockReservationTypeRepository.UpdateTypeAsync(Arg.Any<ReservationType>()).Returns(updatedType);

            var result = await _sut.UpdateReservationType(id, request);

            result.Should().NotBeNull();
            result.Name.Should().Be("Updated");
        }

        [Fact]
        public async Task ThrowsDeleteNotPermittedException_DeleteReservationType_WhenFutureReservationsExist()
        {
            var id = 1;
            var type = ReservationTypeServiceGenerator.GenerateReservationType(1, "A", "Type A", new TimeOnly(10, 0), new TimeOnly(12, 0));

            _mockReservationTypeRepository.GetTypeById(id)
                .Returns(type);
            _mockReservationRepository.GetReservationByTypeIdAfterTodayAsync(id)
                .Returns(new List<Reservation> { new Reservation() });

            var act = async () => await _sut.DeleteReservationType(id);

            await act.Should().ThrowAsync<DeleteNotPermittedException>()
                .WithMessage("Cannot delete A because exits future reservations with this type");
        }

        [Fact]
        public async Task DeletesSuccessfully_DeleteReservationType_WhenNoFutureReservationsExist()
        {
            var id = 1;
            var type = ReservationTypeServiceGenerator.GenerateReservationType(1, "A", "Type A", new TimeOnly(10, 0), new TimeOnly(12, 0));
            _mockReservationTypeRepository.GetTypeById(id)
                .Returns(type);
            _mockReservationRepository.GetReservationByTypeIdAfterTodayAsync(id)
                .Returns(Enumerable.Empty<Reservation>().ToList());

            await _sut.DeleteReservationType(id);

            await _mockReservationTypeRepository.Received(1).DeleteTypeAsync(type);
        }
    }
}
