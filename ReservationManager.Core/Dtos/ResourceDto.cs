namespace ReservationManager.Core.Dtos
{
    public class ResourceDto : UpsertResourceDto
    {
        public int Id { get; set; }
        public IEnumerable<ResourceReservedDto>? ResourceReservedDtos { get; set; }
    }
}
