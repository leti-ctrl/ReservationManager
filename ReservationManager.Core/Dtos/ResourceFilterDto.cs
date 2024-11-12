namespace ReservationManager.Core.Dtos
{
    public class ResourceFilterDto
    {
        public int? TypeId { get; set; }
        public int? ResoruceId { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
        public string? TimeFrom { get; set; }
        public string? TimeTo { get; set; }
    }
}
