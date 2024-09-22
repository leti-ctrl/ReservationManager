namespace ReservationManager.API.Request.Resource
{
    public class ResourceUpsertRequest
    {
        public required string Description { get; set; }
        public required string Type { get; set; }
    }
}
