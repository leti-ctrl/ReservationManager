namespace ReservationManager.API.Request.ReservationType
{
    public class ReservationTypeUpsertRequest
    {
        public required string Code { get; set; }
        public required string StartTime {  get; set; }
        public required string EndTime { get; set; }
    }
}
