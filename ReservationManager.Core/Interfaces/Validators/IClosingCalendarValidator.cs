using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Validators;

public interface IClosingCalendarValidator
{
    Task<bool> ValidateIfAlreadyExistsClosingCalendar(ClosingCalendarDto closingCalendar, int? id);
    bool ValidateClosingCalendarBucket(BulkClosingCalendarDto bulkClosingCalendar);
}