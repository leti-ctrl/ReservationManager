using ReservationManager.Core.Dtos;
using System;

public class ResourceFilterDtoGenerator
{
    public ResourceFilterDto CreateValidFilter(int? typeId = null, int? resourceId = null, DateOnly? day = null, TimeOnly? timeFrom = null, TimeOnly? timeTo = null)
    {
        return new ResourceFilterDto
        {
            TypeId = typeId,
            ResourceId = resourceId,
            Day = day ?? DateOnly.FromDateTime(DateTime.Now),
            TimeFrom = timeFrom ?? TimeOnly.FromDateTime(DateTime.Now),
            TimeTo = timeTo ?? TimeOnly.FromDateTime(DateTime.Now.AddHours(1))
        };
    }

    public ResourceFilterDto CreateInvalidFilter()
    {
        return new ResourceFilterDto
        {
            TypeId = null,
            ResourceId = null,
            Day = null,
            TimeFrom = null,
            TimeTo = null
        };
    }
}