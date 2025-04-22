using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Services
{
    public class ClosingCalendarService(
        IClosingCalendarRepository closingCalendarRepository,
        IClosingCalendarFilterService closingCalendarFilterService,
        IResourceValidator resourceValidator,
        IClosingCalendarValidator closingCalendarValidator,
        IResourceService resourceService)
        : IClosingCalendarService
    {
        public async Task<IEnumerable<ClosingCalendarDto>> GetAllFromToday()
        {
            var list = await closingCalendarRepository.GetAllFromToday();
            return list.Select(x => x.Adapt<ClosingCalendarDto>());
        }

        public async Task<IEnumerable<ClosingCalendarDto>> GetFiltered(ClosingCalendarFilterDto filter)
        {
            return await closingCalendarFilterService.GetFiltered(filter);
        }

        public async Task<ClosingCalendarDto> Create(ClosingCalendarDto closingCalendarDto)
        {
            if (!await resourceValidator.ExistingResouceId(closingCalendarDto.ResourceId))
                throw new CreateNotPermittedException("Resource id does not exists.");

            if (await closingCalendarValidator.ExistingClosignCalendar(closingCalendarDto.ResourceId, closingCalendarDto.Day, null))
                throw new CreateNotPermittedException("This closing calendar is already exists.");

            var model = closingCalendarDto.Adapt<ClosingCalendar>();
            var newEntity = await closingCalendarRepository.CreateEntityAsync(model);
            return newEntity.Adapt<ClosingCalendarDto>();
        }


        public async Task<IEnumerable<ClosingCalendarDto>> BulkCreate(BulkClosingCalendarDto bulkClosingCalendarDto)
        {
            if (!await resourceValidator.ValidateResourceType(bulkClosingCalendarDto.ResourceTypeId))
                throw new CreateNotPermittedException(
                    $"Resource type {bulkClosingCalendarDto.ResourceTypeId} does not exist.");

            var resources = (await resourceService.GetFilteredResources(
                new ResourceFilterDto(){TypeId = bulkClosingCalendarDto.ResourceTypeId})).ToList();

            var daysRange = Enumerable.Range(0, bulkClosingCalendarDto.To.DayNumber - bulkClosingCalendarDto.From.DayNumber + 1)
                .Select(offset => bulkClosingCalendarDto.From.AddDays(offset))
                .ToList();

            var existingClosingCalendars = (await closingCalendarRepository
                .GetExistingClosingCalendars(resources.Select(r => r.Id), daysRange)).ToList();

            var newClosingCalendars = new List<ClosingCalendar>();
            foreach (var resource in resources)
            {
                foreach (var day in daysRange)
                {
                    if (existingClosingCalendars.Any(e => e.ResourceId == resource.Id && e.Day == day))
                        continue;

                    var closingCalendar = new ClosingCalendar
                    {
                        ResourceId = resource.Id,
                        Day = day,
                        Description = bulkClosingCalendarDto.Description,
                    };
                    newClosingCalendars.Add(closingCalendar);
                }
            }

            var createdEntities = await closingCalendarRepository.BulkCreateEntitiesAsync(newClosingCalendars);
            return createdEntities.Select(entity => entity.Adapt<ClosingCalendarDto>());
        }

        public async Task<ClosingCalendarDto> Update(int id, ClosingCalendarDto closingCalendarDto)
        {
            if (!await resourceValidator.ExistingResouceId(closingCalendarDto.ResourceId))
                throw new CreateNotPermittedException("Resource id does not exists.");

            if (await closingCalendarValidator.ExistingClosignCalendar(closingCalendarDto.ResourceId, closingCalendarDto.Day, id))
                throw new CreateNotPermittedException("This closing calendar is already exists.");

            var closingCalendar = closingCalendarDto.Adapt<ClosingCalendar>();
            closingCalendar.Id = id;
            var updated = await closingCalendarRepository.UpdateEntityAsync(closingCalendar);
            return updated.Adapt<ClosingCalendarDto>();
        }

        public async Task Delete(int id)
        {
            var entity = await closingCalendarRepository.GetEntityByIdAsync(id) ??
                         throw new EntityNotFoundException($"Closing Calendar {id} not found.");

            await closingCalendarRepository.DeleteEntityAsync(entity);
        }
    }
}