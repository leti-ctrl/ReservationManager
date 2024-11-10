namespace ReservationManager.Core.Dtos
{
    public class UpsertResourceDto
    {
        public required string Description { get; set; }
        public required int TypeId { get; set; }
    }
}
