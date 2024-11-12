namespace ReservationManager.Core.Dtos
{
    public class ResourceReservedDto
    {
        public DateOnly Day { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
    }
}
