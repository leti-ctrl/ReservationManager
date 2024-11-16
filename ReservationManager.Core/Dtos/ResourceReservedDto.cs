namespace ReservationManager.Core.Dtos
{
    public class ResourceReservedDto
    {
        public DateOnly Day { get; set; }
        public TimeOnly TimeStart { get; set; }
        public TimeOnly TimeEnd { get; set; }
    }
}
