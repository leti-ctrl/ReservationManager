using ReservationManager.DomainModel.Operation;

namespace ReservationManager.DomainModel.Daos;

public class ResourceRepoDao
{
    public int Id { get; set; }  // solo Id
    public string Description { get; set; }
    public List<ResourceReservedRepoDao>? ResourceReservedDtos { get; set; }
}

public class ResourceReservedRepoDao
{
    public bool IsClosed { get; set; }
    public DateOnly Day { get; set; }
    public TimeOnly? TimeStart { get; set; }
    public TimeOnly? TimeEnd { get; set; }
    public int? ReservationId { get; set; }
}
