using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Validators;

public class ResourceValidator : IResourceValidator
{
    private readonly IResourceTypeRepository _resourceTypeRepository;

    public ResourceValidator(IResourceTypeRepository resourceTypeRepository)
    {
        _resourceTypeRepository = resourceTypeRepository;
    }

    public async Task<bool> ValidateResourceType(int typeId)
    {
        return await _resourceTypeRepository.GetTypeById(typeId) != null;
    }


}