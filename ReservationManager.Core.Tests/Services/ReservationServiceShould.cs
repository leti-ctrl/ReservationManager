using FluentAssertions;
using Mapster;
using NSubstitute;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Services;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using Xunit;

[Trait("Category", "Unit")]
public class ReservationServiceShould
{
    private readonly ReservationService _sut;
    private readonly IReservationRepository _mockReservationRepository;
    private readonly IResourceService _mockResourceService;
    private readonly IUpsertReservationValidator _mockUpsertReservationValidator;
    private readonly IReservationTypeRepository _mockReservationTypeRepository;
    private readonly IUserService _mockUserService;

    public ReservationServiceShould()
    {
        _mockReservationRepository = Substitute.For<IReservationRepository>();
        _mockResourceService = Substitute.For<IResourceService>();
        _mockUpsertReservationValidator = Substitute.For<IUpsertReservationValidator>();
        _mockReservationTypeRepository = Substitute.For<IReservationTypeRepository>();
        _mockUserService = Substitute.For<IUserService>();

        _sut = new ReservationService(
            _mockReservationRepository,
            _mockUpsertReservationValidator,
            _mockResourceService,
            _mockReservationTypeRepository,
            _mockUserService
        );
    }

    [Fact]
    public async Task ReturnUserReservations_WhenUserExists()
    {
        var user = new User { Id = 1, Email = "test@mail.com" };
        var session = new SessionInfo(user.Email);
        var reservations = new List<Reservation>
        {
            new Reservation { Id = 1, UserId = user.Id },
            new Reservation { Id = 2, UserId = user.Id }
        };

        _mockUserService.GetUserByEmail(session.UserEmail).Returns(user.Adapt<UserDto>());
        _mockReservationRepository.GetReservationByUserIdFromToday(user.Id).Returns(reservations);

        var result = await _sut.GetUserReservation(session);

        result.Should().NotBeNull();
        result.Should().HaveCount(reservations.Count);
        result.Should().BeEquivalentTo(reservations.Adapt<IEnumerable<ReservationDto>>());
    }

    [Fact]
    public async Task ThrowException_WhenRetrievingReservationsForNonexistentUser()
    {
        var session = new SessionInfo("nonexistent@mail.com");
        _mockUserService.GetUserByEmail(session.UserEmail).Returns((UserDto?)null);

        var act = async () => await _sut.GetUserReservation(session);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot retrieve reservation because user does not exist");
    }

    [Fact]
    public async Task CreateReservation_WhenDataIsValid()
    {
        var user = new User { Id = 1, Email = "test@mail.com" };
        var session = new SessionInfo(user.Email);
        var upsertDto = new UpsertReservationDto
        {
            TypeId = 1,
            ResourceId = 1,
            Day = DateOnly.FromDateTime(DateTime.Now),
            Title = "test"
        };
        var reservationType = new ReservationType
        {
            Id = 1, 
            Code = FixedReservationType.Customizable
        };
        var reservation = upsertDto.Adapt<Reservation>();
        reservation.UserId = user.Id;

        _mockUserService.GetUserByEmail(session.UserEmail).Returns(user.Adapt<UserDto>());
        _mockReservationTypeRepository.GetTypeById(upsertDto.TypeId).Returns(reservationType);
        _mockUpsertReservationValidator.IsDateRangeValid(upsertDto, reservationType).Returns(true);
        _mockReservationRepository.CreateEntityAsync(Arg.Any<Reservation>()).Returns(reservation);

        var result = await _sut.CreateReservation(session, upsertDto);

        result.Should().NotBeNull();
        result.User.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task ThrowException_WhenCreatingReservationForNonexistentUser()
    {
        var session = new SessionInfo("nonexistent@mail.com");
        var upsertDto = new UpsertReservationDto
        {
            TypeId = 1, 
            ResourceId = 1, 
            Title = "testRez"
        };

        _mockUserService.GetUserByEmail(session.UserEmail).Returns((UserDto?)null);

        var act = async () => await _sut.CreateReservation(session, upsertDto);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot create reservation because user does not exist.");
    }

    [Fact]
    public async Task DeleteReservation_WhenReservationExistsAndUserIsOwner()
    {
        var user = new User
        {
            Id = 1, 
            Email = "test@mail.com"
        };
        var session = new SessionInfo(user.Email);
        var reservation = new Reservation
        {
            Id = 1, 
            UserId = user.Id
        };

        _mockUserService.GetUserByEmail(session.UserEmail)
            .Returns(user.Adapt<UserDto>());
        _mockReservationRepository.GetEntityByIdAsync(reservation.Id)
            .Returns(reservation);

        var act = async () => await _sut.DeleteReservation(reservation.Id, session);

        await act.Should().NotThrowAsync();
        await _mockReservationRepository.Received(1).DeleteEntityAsync(reservation);
    }

    [Fact]
    public async Task ThrowException_WhenDeletingNonexistentReservation()
    {
        var testEmail = "test@mail.com";
        var session = new SessionInfo(testEmail);

        _mockUserService.GetUserByEmail(session.UserEmail)
            .Returns(new UserDto() { Id = 1, Email = testEmail, Name = "test", Surname = "test" });
        _mockReservationRepository.GetEntityByIdAsync(999)
            .Returns((Reservation?)null);

        var act = async () => await _sut.DeleteReservation(999, session);

        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("Reservation not found.");
    }

    [Fact]
    public async Task ThrowException_WhenDeletingReservationNotOwnedByUser()
    {
        var user = new User
        {
            Id = 1,
            Email = "test@mail.com"
        };
        var session = new SessionInfo(user.Email);
        var reservation = new Reservation { Id = 1, UserId = 2 };

        _mockUserService.GetUserByEmail(session.UserEmail)
            .Returns(user.Adapt<UserDto>());
        _mockReservationRepository.GetEntityByIdAsync(reservation.Id)
            .Returns(reservation);

        var act = async () => await _sut.DeleteReservation(reservation.Id, session);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot delete because user does not belong to this reservation.");
    }
}