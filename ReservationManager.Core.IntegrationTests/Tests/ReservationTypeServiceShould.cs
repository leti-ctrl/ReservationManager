using FluentAssertions;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.IntegrationTests.Tests;
using static Setup;

public class ReservationTypeServiceShould
{
    private IReservationTypeService _sut;
    private IReservationTypeRepository _reservationTypeRepository;

    public ReservationTypeServiceShould()
    {
        _sut = GetReservationTypeService();
        
        _reservationTypeRepository = GetReservationTypeRepository();
    }
    
    [Test]
    public async Task ReturnsSortedReservationTypes_OnGetAll()
    {
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(20, 30);
        var firstReservationType = new ReservationType(){ Code = "A", Name = "Type A",Start = start,End = end};
        var secondReservationType = new ReservationType(){ Code = "B", Name = "Type B",Start = start,End = end};
        await _reservationTypeRepository.CreateTypeAsync(firstReservationType);
        await _reservationTypeRepository.CreateTypeAsync(secondReservationType);
        
        var result = await _sut.GetAllReservationType();

        result.Should().NotBeNullOrEmpty();
        
        var rezTypeA = result.FirstOrDefault(x => x.Code == "A");
        rezTypeA.Should().NotBeNull();
        rezTypeA.Name.Should().Be("Type A");
        rezTypeA.Start.Should().Be(start);
        rezTypeA.End.Should().Be(end);
        
        var rezTypeB = result.FirstOrDefault(x => x.Code == "B");
        rezTypeB.Should().NotBeNull();
        rezTypeB.Name.Should().Be("Type B");
        rezTypeB.Start.Should().Be(start);
        rezTypeB.End.Should().Be(end);
    }
    
    [Test]
    public async Task ReturnsCreatedDto_OnCreate_WhenValidDataIsPassed()
    {
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(13, 00);
        var request = new UpsertReservationTypeDto() 
            { Code = "WFV", Name = "Type WFV" , StartTime = start, EndTime = end };
        
        var result = await _sut.CreateReservationType(request);

        result.Should().NotBeNull();
        result.Code.Should().Be("WFV");
        result.Name.Should().Be("Type WFV");
        result.Start.Should().Be(start);
        result.End.Should().Be(end);
    }
    
    [Test]
    public async Task ThrowsInvalidCodeTypeException_OnCreate_WhenCodeExists()
    {
        var sut = GetReservationTypeService();
        var repository = GetReservationTypeRepository();
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(13, 00);
        var entity = new ReservationType() 
            { Code = "PAW", Name = "Type PAW" , Start = start, End = end };
        await repository.CreateTypeAsync(entity);
        var request = new UpsertReservationTypeDto() 
            { Code = "PAW", Name = "Type PAW" , StartTime = start, EndTime = end };
        
        var result = async () => await sut.CreateReservationType(request);

        await result.Should().ThrowAsync<InvalidCodeTypeException>()
            .WithMessage("Reservation type with code PAW already exists");
    }
    
    [Test]
    public async Task ReturnsUpdatedDto_OnUpdate_WhenValidDataIsPassed()
    {
        var sut = GetReservationTypeService();
        var repository = GetReservationTypeRepository();
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(13, 00);
        var updatedStart = start.AddHours(2);
        var updatedEnd = end.AddHours(2);
        var request = new UpsertReservationTypeDto() 
            { Code = "ANE", Name = "Updated type ANE" , StartTime = updatedStart, EndTime = updatedEnd };
        var entity = new ReservationType() 
            { Code = "ANE", Name = "Type ANE" , Start = start, End = end };
        var id = (await repository.CreateTypeAsync(entity)).Id;
        
        var result = await sut.UpdateReservationType(id, request);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Code.Should().Be("ANE");
        result.Name.Should().Be("Updated type ANE");
        result.Start.Should().Be(updatedStart);
        result.End.Should().Be(updatedEnd);
    }
    
    [Test]
    public async Task ThrowsDeleteNotPermittedException_OnDelete_WhenFutureReservationsExist()
    {
        var sut = GetReservationTypeService();

        var user = new User()
            { Name = "Name", Surname = "Surname" , Email = "email@email.com" };
        var userId = (await GetUserRepository().CreateEntityAsync(user)).Id;
        
        var resourceType = new ResourceType()
            { Code = "RM", Name = "rooms" };
        var resourceTypeId = (await GetResourceTypeRepository().CreateTypeAsync(resourceType)).Id;
        
        var resource = new Resource()
            { Description = "RED room", TypeId = resourceTypeId };
        var resourceId = (await GetResourceRepository().CreateEntityAsync(resource)).Id;
        
        var reservationType = new ReservationType()
        {
            Code = "VBA",
            Name = "Type VBA",
            Start = new TimeOnly(10, 0),
            End = new TimeOnly(13, 00)
        };
        var reservationTypeId = (await GetReservationTypeRepository().CreateTypeAsync(reservationType)).Id;
        
        var reservation = new Reservation()
        {
            Day = DateOnly.FromDateTime(DateTime.Now).AddDays(10),
            Start = reservationType.Start,
            End = reservationType.End,
            ResourceId = resourceId,
            UserId = userId,
            TypeId = reservationTypeId,
            Description = "stato avanzamento lavori",
            Title = "SAL"
        };
        await GetReservationRepository().CreateEntityAsync(reservation);
        
        var act = async () => await sut.DeleteReservationType(reservationTypeId);

        await act.Should().ThrowAsync<DeleteNotPermittedException>()
            .WithMessage("Cannot delete VBA because exits future reservations with this type");
    }
    
    [Test]
    public async Task DeletesSuccessfully_OnDelete_WhenNoFutureReservationsExist()
    {
        var sut = GetReservationTypeService();
        var repository = GetReservationTypeRepository();
        var reservationType = new ReservationType()
        {
            Code = "B",
            Name = "Type NEW",
            Start = new TimeOnly(10, 0),
            End = new TimeOnly(13, 00)
        };
        var reservationTypeId = (await repository.CreateTypeAsync(reservationType)).Id;

        await sut.DeleteReservationType(reservationTypeId);

        var deleted = await repository.GetTypeById(reservationTypeId);
        deleted.Should().BeNull();
    }
}