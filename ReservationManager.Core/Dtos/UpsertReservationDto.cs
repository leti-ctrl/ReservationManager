namespace ReservationManager.Core.Dtos
{
    public class UpsertReservationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly Day { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public int ResourceId { get; set; }
        public ReservationTypeDto Type { get; set; }
    }
}
