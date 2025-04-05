using FluentValidation;
using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Validators;

public interface IResourceFilterDtoValidator : IValidator<ResourceFilterDto>
{
}