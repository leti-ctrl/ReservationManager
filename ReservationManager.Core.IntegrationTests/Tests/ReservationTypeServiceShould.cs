using FluentAssertions;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.IntegrationTests.Tests;
using static Setup;

public class ReservationTypeServiceShould
{
    [Test]
    public async Task GetAllReservationType_ReturnsSortedReservationTypes()
    {
        var sut = GetReservationTypeService();
        var repository = GetReservationTypeRepository();
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(20, 30);
        var firstReservationType = new ReservationType(){ Code = "A", Name = "Type A",Start = start,End = end};
        var secondReservationType = new ReservationType(){ Code = "B", Name = "Type B",Start = start,End = end};
        await repository.CreateTypeAsync(firstReservationType);
        await repository.CreateTypeAsync(secondReservationType);
        
        var result = await sut.GetAllReservationType();

        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(3);//tiene conto del seed
        
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
    public async Task ReturnsCreatedDto_CreateReservationType_WhenValidRequest()
    {
        var sut = GetReservationTypeService();
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(13, 00);
        var request = new UpsertReservationTypeDto() 
            { Code = "NEW", Name = "Type NEW" , StartTime = start, EndTime = end };
        
        var result = await sut.CreateReservationType(request);

        result.Should().NotBeNull();
        result.Code.Should().Be("NEW");
        result.Name.Should().Be("Type NEW");
        result.Start.Should().Be(start);
        result.End.Should().Be(end);
    }
    
    [Test]
    public async Task ThrowsInvalidCodeTypeException_CreateReservationType_WhenCodeExists()
    {
        var sut = GetReservationTypeService();
        var repository = GetReservationTypeRepository();
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(13, 00);
        var entity = new ReservationType() 
            { Code = "NEW", Name = "Type NEW" , Start = start, End = end };
        await repository.CreateTypeAsync(entity);
        var request = new UpsertReservationTypeDto() 
            { Code = "NEW", Name = "Type NEW" , StartTime = start, EndTime = end };
        
        var result = async () => await sut.CreateReservationType(request);

        await result.Should().ThrowAsync<InvalidCodeTypeException>()
            .WithMessage("Reservation type with code NEW already exists");
    }
    
    [Test]
    public async Task ReturnsUpdatedDto_UpdateReservationType_WhenValidRequest()
    {
        var sut = GetReservationTypeService();
        var repository = GetReservationTypeRepository();
        var id = 999;
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(13, 00);
        var updatedStart = start.AddHours(2);
        var updatedEnd = end.AddHours(2);
        var request = new UpsertReservationTypeDto() 
            { Code = "NEW", Name = "Updated type NEW" , StartTime = updatedStart, EndTime = updatedEnd };
        var entity = new ReservationType() 
            { Id = id, Code = "NEW", Name = "Type NEW" , Start = start, End = end };
        await repository.CreateTypeAsync(entity);
        
        var result = await sut.UpdateReservationType(id, request);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Code.Should().Be("NEW");
        result.Name.Should().Be("Updated type NEW");
        result.Start.Should().Be(updatedStart);
        result.End.Should().Be(updatedEnd);
    }
    
    [Test]
    public async Task ThrowsDeleteNotPermittedException_DeleteReservationType_WhenFutureReservationsExist()
    {
        var sut = GetReservationTypeService();

        var userId = 55;
        var user = new User()
            { Id = userId, Name = "Name", Surname = "Surname" , Email = "email@email.com" };
        await GetUserRepository().CreateEntityAsync(user);
        
        var resourceTypeId = 85;
        var resourceType = new ResourceType()
            { Id = resourceTypeId, Code = "RM", Name = "rooms" };
        await GetResourceTypeRepository().CreateTypeAsync(resourceType);
        
        var resourceId = 1084;
        var resource = new Resource()
            { Id = resourceId, Description = "White room", TypeId = resourceTypeId };
        await GetResourceRepository().CreateEntityAsync(resource);
        
        var reservationTypeId = 1648;
        var reservationType = new ReservationType()
        {
            Id = reservationTypeId,
            Code = "NEW",
            Name = "Type NEW",
            Start = new TimeOnly(10, 0),
            End = new TimeOnly(13, 00)
        };
        await GetReservationTypeRepository().CreateTypeAsync(reservationType);
        
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
            .WithMessage("Cannot delete NEW because exits future reservations with this type");
    }
    
    [Test]
    public async Task DeletesSuccessfully_DeleteReservationType_WhenNoFutureReservationsExist()
    {
        var sut = GetReservationTypeService();
        var repository = GetReservationTypeRepository();
        var reservationTypeId = 1648;
        var reservationType = new ReservationType()
        {
            Id = reservationTypeId,
            Code = "NEW",
            Name = "Type NEW",
            Start = new TimeOnly(10, 0),
            End = new TimeOnly(13, 00)
        };
        await repository.CreateTypeAsync(reservationType);

        await sut.DeleteReservationType(reservationTypeId);

        var deleted = await repository.GetTypeById(reservationTypeId);
        deleted.Should().BeNull();
    }
}