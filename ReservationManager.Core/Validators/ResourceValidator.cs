using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Validators;

public class ResourceValidator : IResourceValidator
{
    private readonly IResourceTypeRepository _resourceTypeRepository;
    private readonly IResourceRepository _resourceRepository;

    public ResourceValidator(IResourceTypeRepository resourceTypeRepository, 
        IResourceRepository resourceRepository)
    {
        _resourceTypeRepository = resourceTypeRepository;
        _resourceRepository = resourceRepository;
    }

    public async Task<bool> ValidateResourceType(int typeId)
    {
        return await _resourceTypeRepository.GetTypeById(typeId) != null;
    }

    public async Task<bool> ExistingResouceId(int id)
    {
        return await _resourceRepository.GetEntityByIdAsync(id) != null;
    }
}