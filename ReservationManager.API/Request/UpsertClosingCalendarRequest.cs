
namespace ReservationManager.API.Request
{
    public class UpsertClosingCalendarRequest
    {
        public DateOnly Day { get; set; }
        public int ResourceId { get; set; }
        public int ResourceTypeId { get; set; }
        public string? Description { get; set; }
    }
}
