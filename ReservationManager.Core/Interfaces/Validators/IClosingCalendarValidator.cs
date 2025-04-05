using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Validators;

public interface IClosingCalendarValidator
{
    Task<bool> ExistingClosignCalendar(int resourceId, DateOnly day, int? closingCalendarId);
}