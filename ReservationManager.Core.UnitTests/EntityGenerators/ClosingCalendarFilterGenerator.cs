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


}