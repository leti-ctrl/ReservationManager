using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Meta;
using System;
using System.Collections.Generic;

namespace Tests.EntityGenerators;

public class ReservationTypeServiceGenerator
{
    public ReservationType GenerateReservationType(int id, string code, string name, TimeOnly start, TimeOnly end)
    {
        return new ReservationType
        {
            Id = id,
            Code = code,
            Name = name,
            Start = start,
            End = end
        };
    }

    public UpsertReservationTypeDto GenerateUpsertDto(string code, string name, TimeOnly start, TimeOnly end)
    {
        return new UpsertReservationTypeDto
        {
            Code = code,
            Name = name,
            StartTime = start,
            EndTime = end
        };
    }

    public IEnumerable<ReservationType> GenerateReservationTypeList()
    {
        return new List<ReservationType>
        {
            GenerateReservationType(1, "A", "Type A", new TimeOnly(10, 0), new TimeOnly(12, 0)),
            GenerateReservationType(2, "B", "Type B", new TimeOnly(9, 0), new TimeOnly(10, 0))
        };
    }
}
