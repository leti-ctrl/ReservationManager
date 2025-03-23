using FluentAssertions;
using Mapster;
using NSubstitute;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Services;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using Tests.EntityGenerators;

namespace Tests.Services;

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
    public async Task ThrowOperationNotPermittedException_GetUserReservation_WhenNonexistentUser()
    {
        var session = new SessionInfo("nonexistent@mail.com");
        _mockUserService.GetUserByEmail(Arg.Any<string>()).Returns((UserDto?)null);

        var act = async () => await _sut.GetUserReservation(session);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot retrieve reservation because user does not exist");
    }
    
    [Fact]
    public async Task ReturnUserReservations_GetUserReservation_WhenUserExists()
    {
        var rezId = new[] {15, 38};
        var userDto = UserDtoGenerator.GenerateValidUserDto();
        var session = new SessionInfo(userDto.Email);
        var reservations = new List<Reservation>
        {
            ReservationGenerator.GenerateValidGivenRezIdAndUserDto(rezId[0], userDto),
            ReservationGenerator.GenerateValidGivenRezIdAndUserDto(rezId[1], userDto)
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(userDto);
        _mockReservationRepository.GetReservationByUserIdFromToday(Arg.Any<int>())
            .Returns(reservations);

        var result = await _sut.GetUserReservation(session);

        result.Should().NotBeNull();
        result.Should().HaveCount(reservations.Count);
        result.First().Id.Should().Be(rezId[0]);
        result.Last().Id.Should().Be(rezId[1]);
        result.First().User.Id.Should().Be(userDto.Id);
        result.Last().User.Id.Should().Be(userDto.Id);
        result.First().User.Email.Should().Be(userDto.Email);
        result.Last().User.Email.Should().Be(userDto.Email);
    }

    [Fact]
    public async Task ThrowOperationNotPermittedException_GetById_WhenNonexistentUser()
    {
        var rezId = 99;
        var session = new SessionInfo("nonexistent@mail.com");
        _mockUserService.GetUserByEmail(Arg.Any<string>()).Returns((UserDto?)null);

        var act = async () => await _sut.GetById(rezId, session);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot retrieve reservation because user does not exist");
    }
    
    [Fact]
    public async Task ReturnNull_GetById_WhenNonexistentReservation()
    {
        var rezId = 99;
        var userDto = UserDtoGenerator.GenerateValidUserDto();
        var session = new SessionInfo(userDto.Email);
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(userDto);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns((Reservation)null!);

        var act = await _sut.GetById(rezId, session);

        act.Should().BeNull();
    }
    
    [Fact]
    public async Task ThrowOperationNotPermittedException_GetById_WhenReservationUserIsNotSessionUser()
    {
        var rezId = 99;
        var rezUserId = 105;
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var reservation = new Reservation()
        {
            Id = rezId,
            UserId = rezUserId,
        };
        var session = new SessionInfo(sessionUser.Email);
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);

        var act = async () => await _sut.GetById(rezId, session);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot retrieve reservation because user does not belong to this reservation.");
    }
    
    [Fact]
    public async Task ReturnReservation_GetById_WhenReservationUserIsSameAsSessionUser()
    {
        var rezId = 99;
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var reservation = ReservationGenerator.GenerateValidGivenRezIdAndUserDto(rezId, sessionUser);
        var session = new SessionInfo(sessionUser.Email);
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);

        var result = await _sut.GetById(rezId, session);

        result.Should().NotBeNull();
        result.Id.Should().Be(rezId);
        result.User.Email.Should().Be(sessionUser.Email);
        result.User.Id.Should().Be(sessionUser.Id);
    }

    [Fact]
    public async Task ThrowOperationNotPermittedException_CreateReservation_WhenNonexistentUser()
    {
        var session = new SessionInfo("nonexistent@mail.com");
        var upsertRez = new UpsertReservationDto() { Title = "test" };
        _mockUserService.GetUserByEmail(Arg.Any<string>()).Returns((UserDto?)null);

        var act = async () => await _sut.CreateReservation(session, upsertRez);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot create reservation because user does not exist.");
    }

    [Fact]
    public async Task ThrowInvalidCodeTypeException_CreateReservation_WhenNonexistentReservationType()
    {
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto() { Title = "test" ,  TypeId = -1 };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns((ReservationType)null!);

        var act = async () => await _sut.CreateReservation(session, upsertRez);

        await act.Should().ThrowAsync<InvalidCodeTypeException>()
            .WithMessage("No reservation type found");
    }
    
    [Fact]
    public async Task ThrowArgumentException_CreateReservation_WhenDateRangeIsNotValid()
    {
        var rezTypeId = 25;
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var rezType = new ReservationType { Id = rezTypeId, Code = "TEST" };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto() { Title = "test" , TypeId = rezTypeId };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(false);

        var act = async () => await _sut.CreateReservation(session, upsertRez);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid reservation");
    }
    
    [Fact]
    public async Task ThrowReservationException_CreateReservation_WhenNonexistentResource()
    {
        var rezTypeId = 25;
        var resourceId = 92;
        var rezTimeStart = new TimeOnly(15, 00, 00);
        var rezTimeEnd = new TimeOnly(18, 00, 00);
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var rezType = new ReservationType
        {
            Id = rezTypeId, 
            Code = "TEST",
            Name = "Test Name",
            Start = rezTimeStart,
            End = rezTimeEnd,
        };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto()
        {
            Title = "test" , 
            TypeId = rezTypeId,
            ResourceId = resourceId,
            Start = rezTimeStart,
            End = rezTimeEnd
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(Enumerable.Empty<ResourceDto>());

        var act = async () => await _sut.CreateReservation(session, upsertRez);

        await act.Should().ThrowAsync<ReservationException>()
            .WithMessage("Invalid resource for reservation");
    }
    
    [Fact]
    public async Task ThrowReservationException_CreateReservation_WhenResourceIsAlreadyReserved()
    {
        var rezTypeId = 25;
        var resourceId = 92;
        var rezDay = new DateOnly(2025, 01, 23);
        var rezTimeStart = new TimeOnly(15, 00, 00);
        var rezTimeEnd = new TimeOnly(18, 00, 00);
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var rezType = new ReservationType
        {
            Id = rezTypeId, 
            Code = "TEST",
            Name = "Test Name",
            Start = rezTimeStart,
            End = rezTimeEnd,
        };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto()
        {
            Title = "test" , 
            TypeId = rezTypeId,
            ResourceId = resourceId,
            Day = rezDay,
            Start = rezTimeStart,
            End = rezTimeEnd
        };
        var resourceDto = new ResourceDto
        {
            Id = resourceId,
            Description = "Test Description",
            ResourceReservedDtos = new List<ResourceReservedDto>()
            {
                new ResourceReservedDto()
                {
                    Day = rezDay,
                    TimeStart = rezTimeStart,
                    TimeEnd = rezTimeEnd,
                    ReservationId = 875,
                }
            }
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(new List<ResourceDto>() { resourceDto });

        var act = async () => await _sut.CreateReservation(session, upsertRez);

        await act.Should().ThrowAsync<ReservationException>()
            .WithMessage("Cannot reserve resource because is closed or is already reserved.");
    }
    
    [Fact]
    public async Task ThrowReservationException_CreateReservation_WhenResourceIsClosed()
    {
        var rezTypeId = 25;
        var resourceId = 92;
        var rezDay = new DateOnly(2025, 01, 23);
        var rezTimeStart = new TimeOnly(15, 00, 00);
        var rezTimeEnd = new TimeOnly(18, 00, 00);
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var rezType = new ReservationType
        {
            Id = rezTypeId, 
            Code = "TEST",
            Name = "Test Name",
            Start = rezTimeStart,
            End = rezTimeEnd,
        };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto()
        {
            Title = "test" , 
            TypeId = rezTypeId,
            ResourceId = resourceId,
            Day = rezDay,
            Start = rezTimeStart,
            End = rezTimeEnd
        };
        var resourceDto = new ResourceDto
        {
            Id = resourceId,
            Description = "Test Description",
            ResourceReservedDtos = new List<ResourceReservedDto>()
            {
                new ResourceReservedDto()
                {
                    Day = rezDay,
                    IsClosed = true,
                }
            }
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(new List<ResourceDto>() { resourceDto });

        var act = async () => await _sut.CreateReservation(session, upsertRez);

        await act.Should().ThrowAsync<ReservationException>()
            .WithMessage("Cannot reserve resource because is closed or is already reserved.");
    }
    
    [Fact]
    public async Task ReturnReservation_CreateReservation_WhenResourceIsFreeAndOpen()
    {
        var rezId = 85;
        var rezTitle = "Test title";
        var rezDescription = "Test Description";
        var rezTypeId = 25;
        var resourceId = 92;
        var rezDay = new DateOnly(2025, 01, 23);
        var rezTimeStart = new TimeOnly(15, 00, 00);
        var rezTimeEnd = new TimeOnly(18, 00, 00);
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var rezType = new ReservationType
        {
            Id = rezTypeId, 
            Code = "TEST",
            Name = "Test Name",
            Start = rezTimeStart,
            End = rezTimeEnd,
        };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto()
        {
            Title = "test" ,
            Description =  rezDescription,
            TypeId = rezTypeId,
            ResourceId = resourceId,
            Day = rezDay,
            Start = rezTimeStart,
            End = rezTimeEnd
        };
        var reservation = new Reservation()
        {
            Id = rezId,
            Title = rezTitle,
            Description = rezDescription,
            Day = rezDay,
            Start = rezTimeStart,
            End = rezTimeEnd,
            Resource = new Resource() { Id = resourceId },
            User = new User() { Id = sessionUser.Id, Email = sessionUser.Email },
            Type = rezType,
        };
        var resourceDto = new ResourceDto
        {
            Id = resourceId,
            Description = "Test Description",
            ResourceReservedDtos = null,
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(new List<ResourceDto>() { resourceDto });
        _mockReservationRepository.CreateEntityAsync(Arg.Any<Reservation>())
            .Returns(reservation);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);

        var result = await _sut.CreateReservation(session, upsertRez);

        result.Should().NotBeNull();
        result.Id.Should().Be(rezId);
        result.Title.Should().Be(rezTitle);
        result.Description.Should().Be(rezDescription); 
        result.Day.Should().Be(rezDay);
        result.Start.Should().Be(rezTimeStart);
        result.End.Should().Be(rezTimeEnd);
        result.Resource.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(sessionUser.Email);
        result.User.Id.Should().Be(sessionUser.Id);
        result.Type.Id.Should().Be(rezTypeId);
    }
    
    [Fact]
    public async Task ThrowOperationNotPermittedException_UpdateReservation_WhenNonexistentUser()
    {
        var rezId = 93;
        var session = new SessionInfo("nonexistent@mail.com");
        var upsertRez = new UpsertReservationDto() { Title = "test" };
        _mockUserService.GetUserByEmail(Arg.Any<string>()).Returns((UserDto?)null);

        var act = async () => await _sut.UpdateReservation(rezId, session, upsertRez);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot update reservation because user does not exist.");
    }
    
    [Fact]
    public async Task ReturnNull_UpdateReservation_WhenNonexistentReservation()
    {
        var rezId = 93;
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto() { Title = "test" ,  TypeId = -1 };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns((Reservation)null!);

        var result = await _sut.UpdateReservation(rezId, session, upsertRez);

        result.Should().BeNull();
    }
    
    [Fact]
    public async Task ThrowOperationNotPermittedException_UpdateReservation_WhenUserRezIsNotSessionUser()
    {
        var rezId = 93;
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto() { Title = "test" ,  TypeId = -1 };
        var reservation = new Reservation
        {
            Id = rezId, 
            Description = "test",
            UserId = sessionUser.Id + 100,
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);

        var act = async () => await _sut.UpdateReservation(rezId, session, upsertRez);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot update because user does not belong to this reservation.");
    }
    
    [Fact]
    public async Task ThrowInvalidCodeTypeException_UpdateReservation_WhenNonexistentReservationType()
    {
        var rezId = 93;
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto() { Title = "test" ,  TypeId = -1 };
        var reservation = new Reservation
        {
            Id = rezId, 
            Description = "test",
            UserId = sessionUser.Id,
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns((ReservationType)null!);

        var act = async () => await _sut.UpdateReservation(rezId, session, upsertRez);

        await act.Should().ThrowAsync<InvalidCodeTypeException>()
            .WithMessage("No reservation type found");
    }
    
    [Fact]
    public async Task ThrowArgumentException_UpdateReservation_WhenDateRangeIsNotValid()
    {
        var rezId = 93;
        var rezTypeId = 25;
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var rezType = new ReservationType { Id = rezTypeId, Code = "TEST" };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto() { Title = "test" , TypeId = rezTypeId };
        var reservation = new Reservation
        {
            Id = rezId, 
            Description = "test",
            UserId = sessionUser.Id,
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(false);

        var act = async () => await _sut.UpdateReservation(rezId, session, upsertRez);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid reservation");
    }
    
    [Fact]
    public async Task ThrowReservationException_UpdateReservation_WhenNonexistentResource()
    {
        var rezId = 93;
        var rezTypeId = 25;
        var resourceId = 92;
        var rezTimeStart = new TimeOnly(15, 00, 00);
        var rezTimeEnd = new TimeOnly(18, 00, 00);
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var reservation = new Reservation
        {
            Id = rezId, 
            Description = "test",
            UserId = sessionUser.Id,
        };
        var rezType = new ReservationType
        {
            Id = rezTypeId, 
            Code = "TEST",
            Name = "Test Name",
            Start = rezTimeStart,
            End = rezTimeEnd,
        };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto()
        {
            Title = "test" , 
            TypeId = rezTypeId,
            ResourceId = resourceId,
            Start = rezTimeStart,
            End = rezTimeEnd
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(Enumerable.Empty<ResourceDto>());

        var act = async () => await _sut.UpdateReservation(rezId, session, upsertRez);

        await act.Should().ThrowAsync<ReservationException>()
            .WithMessage("Invalid resource for reservation");
    }
    
    [Fact]
    public async Task ThrowReservationException_UpdateReservation_WhenResourceIsAlreadyReserved()
    {
        var rezId = 93;
        var rezTypeId = 25;
        var resourceId = 92;
        var rezDay = new DateOnly(2025, 01, 23);
        var rezTimeStart = new TimeOnly(15, 00, 00);
        var rezTimeEnd = new TimeOnly(18, 00, 00);
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var reservation = new Reservation
        {
            Id = rezId, 
            Description = "test",
            UserId = sessionUser.Id,
        };
        var rezType = new ReservationType
        {
            Id = rezTypeId, 
            Code = "TEST",
            Name = "Test Name",
            Start = rezTimeStart,
            End = rezTimeEnd,
        };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto()
        {
            Title = "test" , 
            TypeId = rezTypeId,
            ResourceId = resourceId,
            Day = rezDay,
            Start = rezTimeStart,
            End = rezTimeEnd
        };
        var resourceDto = new ResourceDto
        {
            Id = resourceId,
            Description = "Test Description",
            ResourceReservedDtos = new List<ResourceReservedDto>()
            {
                new ResourceReservedDto()
                {
                    Day = rezDay,
                    TimeStart = rezTimeStart,
                    TimeEnd = rezTimeEnd,
                    ReservationId = 875,
                }
            }
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(new List<ResourceDto>() { resourceDto });

        var act = async () => await _sut.UpdateReservation(rezId, session, upsertRez);

        await act.Should().ThrowAsync<ReservationException>()
            .WithMessage("Cannot reserve resource because is closed or is already reserved.");
    }
    
    [Fact]
    public async Task ThrowReservationException_UpdateReservation_WhenResourceIsClosed()
    {
        var rezId = 93;
        var rezTypeId = 25;
        var resourceId = 92;
        var rezDay = new DateOnly(2025, 01, 23);
        var rezTimeStart = new TimeOnly(15, 00, 00);
        var rezTimeEnd = new TimeOnly(18, 00, 00);
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var reservation = new Reservation
        {
            Id = rezId, 
            Description = "test",
            UserId = sessionUser.Id,
        };
        var rezType = new ReservationType
        {
            Id = rezTypeId, 
            Code = "TEST",
            Name = "Test Name",
            Start = rezTimeStart,
            End = rezTimeEnd,
        };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto()
        {
            Title = "test" , 
            TypeId = rezTypeId,
            ResourceId = resourceId,
            Day = rezDay,
            Start = rezTimeStart,
            End = rezTimeEnd
        };
        var resourceDto = new ResourceDto
        {
            Id = resourceId,
            Description = "Test Description",
            ResourceReservedDtos = new List<ResourceReservedDto>()
            {
                new ResourceReservedDto()
                {
                    Day = rezDay,
                    IsClosed = true,
                }
            }
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(new List<ResourceDto>() { resourceDto });

        var act = async () => await _sut.UpdateReservation(rezId, session, upsertRez);

        await act.Should().ThrowAsync<ReservationException>()
            .WithMessage("Cannot reserve resource because is closed or is already reserved.");
    }
    
    [Fact]
    public async Task RetrieveReservation_UpdateReservation_WhenResourceIsFreeAndOpen()
    {
        var rezId = 93;
        var rezTypeId = 25;
        var resourceId = 92;
        var rezTitle = "Test title";
        var rezDescription = "Test Description";
        var rezDay = new DateOnly(2025, 01, 23);
        var rezTimeStart = new TimeOnly(15, 00, 00);
        var rezTimeEnd = new TimeOnly(18, 00, 00);
        var sessionUser = UserDtoGenerator.GenerateValidUserDto();
        var oldRez = new Reservation
        {
            Id = rezId, 
            Description = "test",
            UserId = sessionUser.Id,
        };
        var rezType = new ReservationType
        {
            Id = rezTypeId, 
            Code = "TEST",
            Name = "Test Name",
            Start = rezTimeStart,
            End = rezTimeEnd,
        };
        var session = new SessionInfo(sessionUser.Email);
        var upsertRez = new UpsertReservationDto()
        {
            Title = rezTitle, 
            Description = rezDescription,
            TypeId = rezTypeId,
            ResourceId = resourceId,
            Day = rezDay,
            Start = rezTimeStart,
            End = rezTimeEnd
        };
        var resourceDto = new ResourceDto
        {
            Id = resourceId,
            Description = "Test Description",
            ResourceReservedDtos = new List<ResourceReservedDto>()
            {
                new ResourceReservedDto()
                {
                    Day = rezDay,
                    TimeStart = rezTimeStart,
                    TimeEnd = rezTimeEnd,
                    ReservationId = rezId,
                }
            }
        };
        var reservation = new Reservation()
        {
            Id = rezId,
            Title = rezTitle,
            Description = rezDescription,
            Day = rezDay,
            Start = rezTimeStart,
            End = rezTimeEnd,
            Resource = new Resource() { Id = resourceId },
            UserId = sessionUser.Id,
            User = new User() { Id = sessionUser.Id, Email = sessionUser.Email },
            Type = rezType,
        };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(sessionUser);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(oldRez);
        _mockReservationTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(rezType);
        _mockUpsertReservationValidator.IsDateRangeValid(Arg.Any<UpsertReservationDto>(), Arg.Any<ReservationType>())
            .Returns(true);
        _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
            .Returns(new List<ResourceDto>() { resourceDto });
        _mockReservationRepository.UpdateEntityAsync(Arg.Any<Reservation>())
            .Returns(reservation);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(reservation);

        var result = await _sut.UpdateReservation(rezId, session, upsertRez);

        result.Should().NotBeNull();
        result.Id.Should().Be(rezId);
        result.Title.Should().Be(rezTitle);
        result.Description.Should().Be(rezDescription); 
        result.Day.Should().Be(rezDay);
        result.Start.Should().Be(rezTimeStart);
        result.End.Should().Be(rezTimeEnd);
        result.Resource.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(sessionUser.Email);
        result.User.Id.Should().Be(sessionUser.Id);
        result.Type.Id.Should().Be(rezTypeId);
    }
    
    [Fact]
    public async Task DeleteReservation_DeleteReservation_WhenReservationExistsAndUserIsOwner()
    {
        var user = UserDtoGenerator.GenerateValidUserDto();
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
    }
    
    [Fact]
    public async Task ThrowOperationNotPermittedException_DeleteReservation_WhenNonexistentUser()
    {
        var userDto = UserDtoGenerator.GenerateValidUserDto();
        var session = new SessionInfo(userDto.Email);
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns((UserDto)null!);
        
        var act = async () => await _sut.DeleteReservation(999, session);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot delete reservation because user does not exist.");
    }

    [Fact]
    public async Task ThrowEntityNotFoundException_DeleteReservation_WhenNonexistentReservation()
    {
        var userDto = UserDtoGenerator.GenerateValidUserDto();
        var session = new SessionInfo(userDto.Email);
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(userDto);
        _mockReservationRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns((Reservation?)null);

        var act = async () => await _sut.DeleteReservation(999, session);

        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("Reservation not found.");
    }

    [Fact]
    public async Task ThrowOperationNotPermittedException_DeleteReservation_WhenReservationNotOwnedByUser()
    {
        var user = UserDtoGenerator.GenerateValidUserDto();
        var session = new SessionInfo(user.Email);
        var reservation = new Reservation { Id = 1, UserId = user.Id + 100 };
        _mockUserService.GetUserByEmail(Arg.Any<string>())
            .Returns(user.Adapt<UserDto>());
        _mockReservationRepository.GetEntityByIdAsync(reservation.Id)
            .Returns(reservation);

        var act = async () => await _sut.DeleteReservation(reservation.Id, session);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot delete because user does not belong to this reservation.");
    }
}