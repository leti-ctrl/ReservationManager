using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Services
{
    public class ClosingCalendarService : IClosingCalendarService
    {
        private readonly IClosingCalendarRepository _closingCalendarRepository;
        private readonly IClosingCalendarFilterValidator _closingCalendarFilterValidator;
        private readonly IResourceValidator _resourceValidator;
        private readonly IClosingCalendarValidator _closingCalendarValidator;
        private readonly IResourceService _resourceService;

        public ClosingCalendarService(IClosingCalendarRepository closingCalendarRepository, 
            IClosingCalendarFilterValidator closingCalendarFilterValidator, 
            IResourceValidator resourceValidator, 
            IClosingCalendarValidator closingCalendarValidator, 
            IResourceService resourceService)
        {
            _closingCalendarRepository = closingCalendarRepository;
            _closingCalendarFilterValidator = closingCalendarFilterValidator;
            _resourceValidator = resourceValidator;
            _closingCalendarValidator = closingCalendarValidator;
            _resourceService = resourceService;
        }

        public async Task<IEnumerable<ClosingCalendarDto>> GetAllFromToday()
        {
            var list = await _closingCalendarRepository.GetAllFromToday();
            return list.Select(x => x.Adapt<ClosingCalendarDto>());
        }

        public async Task<IEnumerable<ClosingCalendarDto>> GetFiltered(ClosingCalendarFilterDto filter)
        {
            if (!_closingCalendarFilterValidator.IsLegalDateRange(filter))
                throw new InvalidFiltersException("You cannot set end date without a start date.");
            
            var closingCalendars = (await _closingCalendarRepository.GetFiltered(filter.Id, 
                filter.StartDay, filter.EndDay, filter.RescourceId, filter.ResourceTypeId)).ToList();

            if (closingCalendars.Any())
                return closingCalendars.Select(x => x.Adapt<ClosingCalendarDto>());
            return Enumerable.Empty<ClosingCalendarDto>();
        }

        public async Task<ClosingCalendarDto> Create(ClosingCalendarDto closingCalendarDto)
        {
            if(!await _resourceValidator.ExistingResouceId(closingCalendarDto.ResourceId))
                throw new CreateClosingCalendarException("Resource id does not exists.");

            if(await _closingCalendarValidator.ValidateIfAlreadyExistsClosingCalendar(closingCalendarDto, null))
                throw new CreateClosingCalendarException("This closing calendar is already exists.");

            var model = closingCalendarDto.Adapt<ClosingCalendar>();
            var newEntity = await _closingCalendarRepository.CreateEntityAsync(model);
            return newEntity.Adapt<ClosingCalendarDto>();
        }

        public async Task<IEnumerable<ClosingCalendarDto>> CreateBucket(ClosingCalendarBucketDto closingCalendarBucketDto)
        {
            if(!await _resourceValidator.ValidateResourceType(closingCalendarBucketDto.ResourceTypeId))
                throw new CreateClosingCalendarException($"Resource type {closingCalendarBucketDto.ResourceTypeId} does not exists.");

            var resources = await _resourceService.GetFilteredResources(new ResourceFilterDto()
                { TypeId = closingCalendarBucketDto.ResourceTypeId });

            var bucketClosingCalendar = new List<ClosingCalendarDto>();
            foreach (var resource in resources)
            {
                for (var day = closingCalendarBucketDto.From; day <= closingCalendarBucketDto.To; day = day.AddDays(1))
                {
                    var closingCalendarDto = new ClosingCalendarDto()
                    {
                        ResourceId = resource.Id,
                        Day = day,
                        Description = closingCalendarBucketDto.Description,
                    };
                    
                    if(await _closingCalendarValidator.ValidateIfAlreadyExistsClosingCalendar(closingCalendarDto, null))
                        continue;
                    
                    var newEntity = await _closingCalendarRepository.CreateEntityAsync(closingCalendarDto.Adapt<ClosingCalendar>());
                    bucketClosingCalendar.Add(newEntity.Adapt<ClosingCalendarDto>());
                }
            }
            return bucketClosingCalendar;
        }

        public async Task<ClosingCalendarDto> Update(int id, ClosingCalendarDto closingCalendarDto)
        {
            if(!await _resourceValidator.ExistingResouceId(closingCalendarDto.ResourceId))
                throw new CreateClosingCalendarException("Resource id does not exists.");

            if(await _closingCalendarValidator.ValidateIfAlreadyExistsClosingCalendar(closingCalendarDto, id))
                throw new CreateClosingCalendarException("This closing calendar is already exists.");
                
            var closingCalendar = closingCalendarDto.Adapt<ClosingCalendar>();
            closingCalendar.Id = id;
            var updated = await _closingCalendarRepository.UpdateEntityAsync(closingCalendar);
            return updated.Adapt<ClosingCalendarDto>();
        }

        public async Task Delete(int id)
        {
            var entity = await _closingCalendarRepository.GetEntityByIdAsync(id) ??
                         throw new EntityNotFoundException($"Closing Calendar {id} not found.");
            
            await _closingCalendarRepository.DeleteEntityAsync(entity);
        }
    }
}
