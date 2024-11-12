using FluentValidation;
using FluentValidation.Results;
using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces;

public interface IResourceFilterValidator : IValidator<ResourceFilterDto>
{
    
}