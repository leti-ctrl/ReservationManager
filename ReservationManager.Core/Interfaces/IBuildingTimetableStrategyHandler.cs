using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Interfaces
{
    public interface IBuildingTimetableStrategyHandler
    {
        Task<BuildingTimetable> CreateTimetable(UpsertEstabilishmentTimetableDto entity);
        Task<BuildingTimetable> UpdateTimetable(UpsertEstabilishmentTimetableDto entity, int id);
    }
}
