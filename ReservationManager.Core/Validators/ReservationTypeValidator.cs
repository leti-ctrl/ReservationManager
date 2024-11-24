using FluentValidation;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Validators;

public class ReservationTypeValidator : AbstractValidator<ReservationType>, IReservationTypeValidator
{
    public ReservationTypeValidator()
    {
        RuleFor(reservationType => reservationType.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(reservationType => reservationType.Code)
            .NotEmpty()
            .WithMessage("Code is required");
        
        RuleFor(reservationType => reservationType.Start)
            .NotEmpty()
            .WithMessage("Start is required");
        
        RuleFor(reservationType => reservationType.End)
            .NotEmpty()
            .WithMessage("End is required");
        
        RuleFor(x => x.Start)
            .LessThan(x => x.End)
            .WithMessage("Start cannot be greater than end");
            
    }
}