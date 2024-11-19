namespace ReservationManager.Core.Interfaces.Validators;

public interface IResourceValidator
{
    Task<bool> ValidateResourceType(int typeId);
    Task<bool> ExistingResouceId(int id);
}