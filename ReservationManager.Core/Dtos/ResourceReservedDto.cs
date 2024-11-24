namespace ReservationManager.Core.Dtos
{
    public class ResourceReservedDto
    {
        public bool IsClosed { get; set; }
        public DateOnly Day { get; set; }
        public TimeOnly TimeStart { get; set; }
        public TimeOnly TimeEnd { get; set; }
    }
}
