namespace ReservationManager.API.Request
{
    public class ReservationTypeUpsertRequest
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
    }
}
