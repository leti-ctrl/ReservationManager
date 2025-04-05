using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using System;
using System.Collections.Generic;
using ReservationManager.Core.Dtos;

namespace Tests.EntityGenerators;

public class ResourceGenerator
{
    public Resource GenerateResource(int id, int typeId)
    {
        return new Resource
        {
            Id = id,
            Description = $"Test Resource {id}",
            TypeId = typeId,
            Type = new ResourceType { Id = typeId, Name = $"Type {typeId}", Code = $"T{typeId}" }
        };
    }

    public List<Resource> GenerateResourceList(int count, int typeId)
    {
        var resources = new List<Resource>();
        for (int i = 1; i <= count; i++)
        {
            resources.Add(GenerateResource(i, typeId));
        }
        return resources;
    }
    
    public List<Resource> GenerateResourceList(int count)
    {
        var resources = new List<Resource>();
        for (int i = 1; i <= count; i++)
        {
            resources.Add(GenerateResource(i, i));
        }
        return resources;
    }
    
    public UpsertResourceDto GenerateUpsert(int typeId, string description)
    {
        return new UpsertResourceDto { TypeId = typeId, Description = description };
    }
    
    public ResourceFilterDto GenerateValidFilter(int? typeId = null, int? resourceId = null, DateOnly? day = null, TimeOnly? timeFrom = null, TimeOnly? timeTo = null)
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

    public ResourceFilterDto GenerateInvalidFilter()
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