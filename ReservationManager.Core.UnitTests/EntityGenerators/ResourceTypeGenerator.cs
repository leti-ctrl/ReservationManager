using ReservationManager.DomainModel.Meta;

namespace Tests.EntityGenerators;

public class ResourceTypeGenerator
{
    public List<ResourceType> GenerateList()
    {
        return new List<ResourceType>
        {
            new ResourceType { Id = 1, Code = "RT1", Name = "Resource Type 1" },
            new ResourceType { Id = 2, Code = "RT2", Name = "Resource Type 2" }
        };
    }
    
    public ResourceType GenerateSingle()
    {
        return new ResourceType { Id = 1, Code = "RT1", Name = "Name" };
    }
}