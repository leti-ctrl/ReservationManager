using ReservationManager.Core.Dtos;

namespace Tests.EntityGenerators;

public class ResourceFilterDtoValidModelGenerator : TheoryData<ResourceFilterDto>
{
    public ResourceFilterDtoValidModelGenerator()
    {
        Add(new ResourceFilterDto()
        {
            TypeId = 1,
            ResourceId = 1,
            Day = new DateOnly(2025, 05, 05),
            TimeFrom = new TimeOnly(16, 00, 00),
            TimeTo = new TimeOnly(17, 00, 00),
        });
        Add(new ResourceFilterDto()
        {
            TypeId = null,
            ResourceId = 1,
            Day = new DateOnly(2025, 05, 05),
            TimeFrom = new TimeOnly(16, 00, 00),
            TimeTo = new TimeOnly(17, 00, 00),
        });
        Add(new ResourceFilterDto()
        {
            TypeId = 1,
            ResourceId = null,
            Day = new DateOnly(2025, 05, 05),
            TimeFrom = new TimeOnly(16, 00, 00),
            TimeTo = new TimeOnly(17, 00, 00),
        });
        Add(new ResourceFilterDto()
        {
            TypeId = 1,
            ResourceId = null,
            Day = null,
            TimeFrom = null,
            TimeTo = null,
        });
        Add(new ResourceFilterDto()
        {
            TypeId = null,
            ResourceId = 1,
            Day = null,
            TimeFrom = null,
            TimeTo = null,
        });
        Add(new ResourceFilterDto()
        {
            TypeId = 1,
            ResourceId = 1,
            Day = null,
            TimeFrom = null,
            TimeTo = null,
        });
    }
}