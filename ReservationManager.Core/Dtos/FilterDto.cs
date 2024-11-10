namespace ReservationManager.Core.Dtos
{
    public class FilterDto
    {
        public int? TypeId { get; set; }
        public int? ResoruceId { get; set; }
        public IEnumerable<DateOnly>? Days { get; set; }
        public IEnumerable<TimeOnly>? Times { get; set; }
    }
}
