
namespace ReservationManager.API.Request
{
    public class UpsertClosingCalendarRequest
    {
        public int TypeId { get; set; }
        public DateOnly Day { get; set; }
        public int ResourceId { get; set; }
        public string? Description { get; set; }
    }
}
