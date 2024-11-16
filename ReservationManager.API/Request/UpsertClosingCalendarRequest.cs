
namespace ReservationManager.API.Request
{
    public class UpsertClosingCalendarRequest
    {
        public int TypeId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string? Description { get; set; }
    }
}
