namespace ReservationManager.Core.Dtos
{
    public class ReservationTypeDto
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }
}
