using ReservationManager.Core.Dtos;

namespace Tests.EntityGenerators;

public class ClosingCalendarFilterGenerator
{
    public ClosingCalendarFilterDto GenerateValidFilter()
    {
        return new ClosingCalendarFilterDto
        {
            StartDay = DateOnly.FromDateTime(DateTime.Now),
            EndDay = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        };
    }

    public ClosingCalendarFilterDto GenerateInvalidFilter()
    {
        return new ClosingCalendarFilterDto
        {
            StartDay = null,
            EndDay = DateOnly.FromDateTime(DateTime.Now)
        };
    }

    public ClosingCalendarFilterDto GenerateFilter()
    {
        return new ClosingCalendarFilterDto
        {
            Id = 1,
            StartDay = DateOnly.FromDateTime(DateTime.Now),
            EndDay = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            RescourceId = 2,
            ResourceTypeId = 3
        };
    }
}