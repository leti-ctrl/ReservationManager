using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface IBuildingTimetableStrategyHandler
    {
        Task<BuildingTimetable> BuildTimetable(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type);
    }
}
