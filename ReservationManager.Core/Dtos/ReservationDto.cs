namespace ReservationManager.Core.Dtos
{
    public class ReservationDto 
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly Day { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public required ResourceDto Resource { get; set; }
        public required UserDto User { get; set; }
        public required ReservationTypeDto Type { get; set; }
    }
}
