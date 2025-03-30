using FluentAssertions;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Mappers;
using ReservationManager.DomainModel.Operation;

namespace Tests.Mappers;

[Trait("Category", "Unit")]
public class ResourceReservedMapperShould
{
    private readonly ResourceReservedMapper _sut = new();

    [Fact]
    public void MapResources_WithReservationsAndClosings_ReturnsCorrectlyMappedResources()
    {
        var resourceId = 1;
        var anotherResourceId = 2;
        
        var resources = new List<Resource>
        {
            new Resource { Id = resourceId, Description = "Resource 1" },
            new Resource { Id = anotherResourceId, Description = "Resource 2" }
        };

        var reservations = new List<Reservation>
        {
            new Reservation { Id = 10, ResourceId = resourceId, Day = new DateOnly(2024, 3, 20), Start = new TimeOnly(10, 0), End = new TimeOnly(11, 0) },
            new Reservation { Id = 11, ResourceId = resourceId, Day = new DateOnly(2024, 3, 21), Start = new TimeOnly(14, 0), End = new TimeOnly(15, 0) }
        };

        var closingCalendars = new List<ClosingCalendarDto>
        {
            new ClosingCalendarDto { ResourceId = resourceId, Day = new DateOnly(2024, 3, 22) },
            new ClosingCalendarDto { ResourceId = anotherResourceId, Day = new DateOnly(2024, 3, 23) }
        };

        // Act
        var result = _sut.Map(resources, reservations, closingCalendars).ToList();

        // Assert
        result.Should().HaveCount(2);

        var resource1 = result.First(r => r.Id == resourceId);
        resource1.ResourceReservedDtos.Should().HaveCount(3);

        resource1.ResourceReservedDtos.Should().ContainEquivalentOf(new ResourceReservedDto
        {
            IsClosed = false,
            Day = new DateOnly(2024, 3, 20),
            TimeStart = new TimeOnly(10, 0),
            TimeEnd = new TimeOnly(11, 0),
            ReservationId = 10
        });

        resource1.ResourceReservedDtos.Should().ContainEquivalentOf(new ResourceReservedDto
        {
            IsClosed = false,
            Day = new DateOnly(2024, 3, 21),
            TimeStart = new TimeOnly(14, 0),
            TimeEnd = new TimeOnly(15, 0),
            ReservationId = 11
        });

        resource1.ResourceReservedDtos.Should().ContainEquivalentOf(new ResourceReservedDto
        {
            IsClosed = true,
            Day = new DateOnly(2024, 3, 22),
            TimeStart = null,
            TimeEnd = null,
            ReservationId = null
        });

        var resource2 = result.First(r => r.Id == anotherResourceId);
        resource2.ResourceReservedDtos.Should().HaveCount(1);

        resource2.ResourceReservedDtos.Should().ContainEquivalentOf(new ResourceReservedDto
        {
            IsClosed = true,
            Day = new DateOnly(2024, 3, 23),
            TimeStart = null,
            TimeEnd = null,
            ReservationId = null
        });
    }
}