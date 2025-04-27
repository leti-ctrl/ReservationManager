using ReservationManager.DomainModel.Operation;

namespace ReservationManager.DomainModel.Dtos;

public class ResourceRepoDto
{
    public int Id { get; set; }  // solo Id
    public string Description { get; set; }
    public List<ResourceReservedRepoDto>? ResourceReservedDtos { get; set; }
}

public class ResourceReservedRepoDto
{
    public bool IsClosed { get; set; }
    public DateOnly Day { get; set; }
    public TimeOnly? TimeStart { get; set; }
    public TimeOnly? TimeEnd { get; set; }
    public int? ReservationId { get; set; }
}
