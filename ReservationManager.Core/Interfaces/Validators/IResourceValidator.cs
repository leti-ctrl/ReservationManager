namespace ReservationManager.Core.Interfaces.Validators;

public interface IResourceValidator
{
    Task<bool> ValidateResourceType(int typeId);
}