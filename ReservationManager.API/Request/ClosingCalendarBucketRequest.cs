namespace ReservationManager.API.Request;

public class ClosingCalendarBucketRequest
{
    public int ResourceTypeId { get; set; }
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public string Description { get; set; }
}