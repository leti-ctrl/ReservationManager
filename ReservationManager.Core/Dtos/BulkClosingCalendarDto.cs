namespace ReservationManager.Core.Dtos;

public class BulkClosingCalendarDto
{
    public int ResourceTypeId { get; set; }
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public string Description { get; set; }
}