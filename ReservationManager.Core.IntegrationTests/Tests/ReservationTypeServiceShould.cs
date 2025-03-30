using FluentAssertions;
using ReservationManager.DomainModel.Meta;

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
}