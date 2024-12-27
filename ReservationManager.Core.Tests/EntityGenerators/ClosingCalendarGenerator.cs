using ReservationManager.DomainModel.Operation;

namespace Tests.EntityGenerators;

public class ClosingCalendarGenerator
{
    public List<ClosingCalendar> GenerateList()
    {
        return new List<ClosingCalendar>
        {
            new ClosingCalendar { Day = DateOnly.FromDateTime(DateTime.Now.AddDays(2)) },
            new ClosingCalendar { Day = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) },
            new ClosingCalendar { Day = DateOnly.FromDateTime(DateTime.Now) }
        };
    }
}