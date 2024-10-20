using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface IEstabilishmentTimetableBuilderStrategy
    {
        bool IsMatch(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type);
        Task<EstabilishmentTimetable> Build(UpsertEstabilishmentTimetableDto entity);
    }
}
