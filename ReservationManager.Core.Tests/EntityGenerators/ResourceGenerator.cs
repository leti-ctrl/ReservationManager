using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using System;
using System.Collections.Generic;

public class ResourceGenerator
{
    public Resource CreateResource(int id, int typeId)
    {
        return new Resource
        {
            Id = id,
            Description = $"Test Resource {id}",
            TypeId = typeId,
            Type = new ResourceType { Id = typeId, Name = $"Type {typeId}", Code = $"T{typeId}" }
        };
    }

    public List<Resource> CreateResourceList(int count, int typeId)
    {
        var resources = new List<Resource>();
        for (int i = 1; i <= count; i++)
        {
            resources.Add(CreateResource(i, typeId));
        }
        return resources;
    }
}