using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Meta;
using System;
using System.Collections.Generic;

namespace Tests.EntityGenerators;

public static class ReservationTypeServiceGenerator
{
    public static ReservationType GenerateReservationType(int id, string code, string name, TimeOnly start, TimeOnly end)
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

    public static UpsertReservationTypeDto GenerateUpsertDto(string code, string name, TimeOnly start, TimeOnly end)
    {
        return new UpsertReservationTypeDto
        {
            Code = code,
            Name = name,
            StartTime = start,
            EndTime = end
        };
    }
}
