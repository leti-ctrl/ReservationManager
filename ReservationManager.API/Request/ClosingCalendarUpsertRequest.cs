
namespace ReservationManager.API.Request
{
    public class ClosingCalendarUpsertRequest
    {
        public DateOnly Day { get; set; }
        public int ResourceId { get; set; }
        public string? Description { get; set; }
    }
}
