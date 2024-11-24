using FluentValidation;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Interfaces.Validators;

public interface IReservationTypeValidator : IValidator<ReservationType>
{
    
}