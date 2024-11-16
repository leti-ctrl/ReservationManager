using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Interfaces;

public interface IResourceValidator
{
    Task<bool> ValidateResourceType(int typeId);
}