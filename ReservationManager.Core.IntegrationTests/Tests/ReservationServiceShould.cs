using System.Runtime.Intrinsics;
using FluentAssertions;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.IntegrationTests.Tests;
using static Setup;

public class ReservationServiceShould
{
    [Test]
    public async Task ThrowOperationNotPermittedException_GetUserReservation_WhenNonexistentUser()
    {
        var sut = GetReservationService();
        var session = new SessionInfo("nonexistent@mail.com");

        var act = async () => await sut.GetUserReservation(session);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot retrieve reservation because user does not exist");
    }
    
    [Test]
    public async Task ReturnUserReservations_GetUserReservation_WhenUserExists()
    {
        var sut = GetReservationService();

        var userId = 42;
        var userEmail = "user@mail.com";
        var user = new User()
            { Id = userId, Name = "Name", Surname = "Surname" , Email = userEmail };
        await GetUserRepository().CreateEntityAsync(user);

        var session = new SessionInfo(userEmail);

        var resourceTypeId = 864;
        var resourceType = new ResourceType()
            { Id = resourceTypeId, Code = "ROOM", Name = "rooms" };
        await GetResourceTypeRepository().CreateTypeAsync(resourceType);

        var resourceId = 6244;
        var resource = new Resource()
            { Id = resourceId, Description = "White room", TypeId = resourceTypeId };
        await GetResourceRepository().CreateEntityAsync(resource);

        var reservationTypeStart = new TimeOnly(10, 0);
        var reservationTypeEnd = new TimeOnly(13, 0);
        var reservationTypeId = 345;
        var reservationType = new ReservationType()
        {
            Id = reservationTypeId,
            Code = "REZC",
            Name = "Type NEW",
            Start = reservationTypeStart,
            End = reservationTypeEnd
        };
        await GetReservationTypeRepository().CreateTypeAsync(reservationType);
        
        var reservation = new Reservation()
        {
            Day = DateOnly.FromDateTime(DateTime.Now).AddDays(10),
            Start = reservationTypeStart,
            End = reservationTypeEnd,
            ResourceId = resourceId,
            UserId = userId,
            TypeId = reservationTypeId,
            Description = "stato avanzamento lavori",
            Title = "SAL"
        };
        await GetReservationRepository().CreateEntityAsync(reservation);
        
        var result = await sut.GetUserReservation(session);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        var ret = result.First();
        ret.User.Id.Should().Be(userId);
        ret.Resource.Id.Should().Be(resourceId);
        ret.Type.Id.Should().Be(reservationTypeId);
        ret.Start.Should().Be(reservationTypeStart);
        ret.End.Should().Be(reservationTypeEnd);
    }
    
    [Test]
    public async Task ThrowOperationNotPermittedException_GetById_WhenNonexistentUser()
    {
        var rezId = 99;
        var sut = GetReservationService();
        var session = new SessionInfo("nonexistent@mail.com");
        
        var act = async () => await sut.GetById(rezId, session);

        await act.Should().ThrowAsync<OperationNotPermittedException>()
            .WithMessage("Cannot retrieve reservation because user does not exist");
    }
    
    [Test]
    public async Task ReturnNull_GetById_WhenNonexistentReservation()
    {
        var sut = GetReservationService();
        var rezId = 99;
        var userId = 42;
        var userEmail = "user@mail.com";
        var user = new User()
            { Id = userId, Name = "Name", Surname = "Surname" , Email = userEmail };
        await GetUserRepository().CreateEntityAsync(user);
        var session = new SessionInfo(userEmail);

        var act = await sut.GetById(rezId, session);

        act.Should().BeNull();
    }
    
    [Test]
    public async Task ReturnReservation_CreateReservation_WhenResourceIsFreeAndOpen()
    {        
        var sut = GetReservationService();

        var userId = 42;
        var userEmail = "user@mail.com";
        var user = new User()
            { Id = userId, Name = "Name", Surname = "Surname" , Email = userEmail };
        await GetUserRepository().CreateEntityAsync(user);
        var session = new SessionInfo(userEmail);
        
        var resourceTypeId = 864;
        var resourceType = new ResourceType()
            { Id = resourceTypeId, Code = "ROOM", Name = "rooms" };
        await GetResourceTypeRepository().CreateTypeAsync(resourceType);

        var resourceId = 6244;
        var resource = new Resource()
            { Id = resourceId, Description = "White room", TypeId = resourceTypeId };
        await GetResourceRepository().CreateEntityAsync(resource);

        var reservationTypeStart = new TimeOnly(10, 0);
        var reservationTypeEnd = new TimeOnly(13, 0);
        var reservationTypeId = 345;
        var reservationType = new ReservationType()
        {
            Id = reservationTypeId,
            Code = "REZC",
            Name = "Type NEW",
            Start = reservationTypeStart,
            End = reservationTypeEnd
        };
        await GetReservationTypeRepository().CreateTypeAsync(reservationType);
        
        var upsertRez = new UpsertReservationDto()
        {
            Day = DateOnly.FromDateTime(DateTime.Now).AddDays(10),
            ResourceId = resourceId,
            TypeId = reservationTypeId,
            Description = "stato avanzamento lavori",
            Title = "SAL"
        };

        var result = await sut.CreateReservation(session, upsertRez);

        result.Should().NotBeNull();
        result.Title.Should().Be(upsertRez.Title);
        result.Description.Should().Be(upsertRez.Description); 
        result.Day.Should().Be(upsertRez.Day);
        result.Start.Should().Be(reservationType.Start);
        result.End.Should().Be(reservationType.End);
        result.Resource.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.User.Id.Should().Be(userId);
        result.Type.Id.Should().Be(reservationTypeId);
    }
}