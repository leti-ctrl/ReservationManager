using ReservationManager.Core.Dtos;

namespace Tests.EntityGenerators;

public class ResourceFilterDtoInvalidModelGenerator : TheoryData<ResourceFilterDto>
{
    public ResourceFilterDtoInvalidModelGenerator()
    {
        Add(new ResourceFilterDto()
        {
            TypeId = null,
            ResourceId = null,
            Day = new DateOnly(2025, 05, 05),
            TimeFrom = new TimeOnly(16, 00, 00),
            TimeTo = new TimeOnly(17, 00, 00),
        });
        Add(new ResourceFilterDto()
        {
            TypeId = null,
            ResourceId = null,
            Day =  null,
            TimeFrom = null,
            TimeTo = null,
        });
        Add(new ResourceFilterDto()
        {
            TypeId = null,
            ResourceId = null,
            Day =  new DateOnly(2025, 05, 05),
            TimeFrom = null,
            TimeTo = null,
        });
        Add(new ResourceFilterDto()
        {
            TypeId = null,
            ResourceId = null,
            Day =  null,
            TimeFrom = new TimeOnly(16, 00, 00),
            TimeTo = null,
        });
        Add(new ResourceFilterDto()
        {
            TypeId = null,
            ResourceId = null,
            Day =  null,
            TimeFrom = null,
            TimeTo = new TimeOnly(16, 00, 00),
        });
        Add(new ResourceFilterDto()
        {
            TypeId = 1,
            ResourceId = 1,
            Day = new DateOnly(2025, 05, 05),
            TimeFrom = new TimeOnly(18, 00, 00),
            TimeTo = new TimeOnly(17, 00, 00),
        });
        Add(new ResourceFilterDto()
        {
            TypeId = 1,
            ResourceId = 1,
            Day =  new DateOnly(2025, 05, 05),
            TimeFrom = null,
            TimeTo = new TimeOnly(17, 00, 00),
        });
        Add(new ResourceFilterDto()
        {
            TypeId = 1,
            ResourceId = 1,
            Day =  new DateOnly(2025, 05, 05),
            TimeFrom = new TimeOnly(16, 00, 00),
            TimeTo = null,
        });
        Add(new ResourceFilterDto()
        {
            TypeId = 1,
            ResourceId = 1,
            Day =  null,
            TimeFrom = null,
            TimeTo = new TimeOnly(17, 00, 00),
        });
        Add(new ResourceFilterDto()
        {
            TypeId = 1,
            ResourceId = 1,
            Day =  new DateOnly(2025, 05, 05),
            TimeFrom = null,
            TimeTo = null,
        });
    }
}