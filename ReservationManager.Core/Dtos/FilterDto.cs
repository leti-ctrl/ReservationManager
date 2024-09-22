namespace ReservationManager.Core.Dtos
{
    public class FilterDto
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Types { get; set; }
        public IEnumerable<DateOnly> Days { get; set; }
        public IEnumerable<TimeOnly> Times { get; set; }
    }
}
