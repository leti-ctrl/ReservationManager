using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Builders
{
    public class EstabilishmentTimetableBuilderStrategyHandler : IEstabilishmentTimetableBuilderStrategyHandler
    {
        private readonly IEnumerable<IEstabilishmentTimetableBuilderStrategy> _strategies;

        public EstabilishmentTimetableBuilderStrategyHandler(IEnumerable<IEstabilishmentTimetableBuilderStrategy> strategies)
        {
            _strategies = strategies;
        }

        public async Task<EstabilishmentTimetable> BuildTimetable(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type)
        {
            var strategy = _strategies.FirstOrDefault(s => s.IsMatch(entity, type))
                ?? throw new NotImplementedException("Nessuna strategia corrisponde a questo tipo di estabilishment timetable.");

            return await strategy.Build(entity);
        }

    }
}
