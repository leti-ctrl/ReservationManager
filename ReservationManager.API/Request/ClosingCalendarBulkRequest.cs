namespace ReservationManager.API.Request;

public class ClosingCalendarBulkRequest
{
    public int ResourceTypeId { get; set; }
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public string Description { get; set; }
}