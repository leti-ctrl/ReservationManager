namespace ReservationManager.Core.Dtos
{
    public class ResourceReservedDto
    {
        public DateOnly Day { get; set; }
        public IEnumerable<TimeOnly> reservedTimes { get; set; }
    }
}
