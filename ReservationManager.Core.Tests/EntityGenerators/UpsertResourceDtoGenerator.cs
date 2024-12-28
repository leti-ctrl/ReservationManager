using ReservationManager.Core.Dtos;

namespace Tests.EntityGenerators;

public class UpsertResourceDtoGenerator
{
    public UpsertResourceDto Generate(int typeId, string description)
    {
        return new UpsertResourceDto { TypeId = typeId, Description = description };
    }
}