namespace ReservationManager.API.Request;

public class ResourceFilterRequest
{
    public int? TypeId { get; set; }
    public int? ResourceId { get; set; }
    public DateOnly? Day { get; set; }
    public TimeOnly? TimeFrom { get; set; }
    public TimeOnly? TimeTo { get; set; }
}