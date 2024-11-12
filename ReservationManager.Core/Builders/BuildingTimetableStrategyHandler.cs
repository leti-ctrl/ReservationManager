using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Core.Exceptions;

namespace ReservationManager.Core.Builders
{
    public class BuildingTimetableStrategyHandler : IBuildingTimetableStrategyHandler
    {
        private readonly IEnumerable<IBuildingTimetableStrategy> _strategies;

        public BuildingTimetableStrategyHandler(IEnumerable<IBuildingTimetableStrategy> strategies)
        {
            _strategies = strategies;
        }

        public async Task<BuildingTimetable> CreateTimetable(UpsertEstabilishmentTimetableDto entity,
            TimetableTypeDto type)
        {
            var strategy = _strategies.FirstOrDefault(s => s.IsMatch(entity, type))
                ?? throw new StrategyNotFoundException($"No strategy found for type {type.Id}.");

            return await strategy.Create(entity);
        }

        public async Task<BuildingTimetable> UpdateTimetable(UpsertEstabilishmentTimetableDto entity,
            TimetableTypeDto type, int id)
        {
            var strategy = _strategies.FirstOrDefault(s => s.IsMatch(entity, type))
                           ?? throw new StrategyNotFoundException($"No strategy found for type {type.Id}.");

            return await strategy.Update(id, entity);
        }
    }
}
