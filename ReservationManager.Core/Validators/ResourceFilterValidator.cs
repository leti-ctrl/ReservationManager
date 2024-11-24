using FluentValidation;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Validators;

public class ResourceFilterValidator : AbstractValidator<ResourceFilterDto>, IResourceFilterValidator
{
    public ResourceFilterValidator()
    {
        RuleFor(x => x.TypeId)
            .NotNull()
            .When(x => !x.ResourceId.HasValue)
            .WithMessage("TypeId and ResourceId cannot both be null");
        
        RuleFor(x => x.ResourceId)
            .NotNull()
            .When(x => !x.TypeId.HasValue)
            .WithMessage("TypeId and ResourceId cannot both be null");
        
        RuleFor(x => x.Day)
            .NotNull()
            .When(x => x.TimeFrom.HasValue && x.TimeTo.HasValue)
            .WithMessage("If TimeFrom and TimeTo has a value, DateFrom must also be set.");
        
        RuleFor(x => x.TimeFrom)
            .NotNull()
            .When(x => x.Day.HasValue && x.TimeTo.HasValue)
            .WithMessage("If DateFrom and TimeTo has a value, TimeFrom must also be set.");

        RuleFor(x => x.TimeTo)
            .NotNull()
            .When(x => x.Day.HasValue && x.TimeFrom.HasValue)
            .WithMessage("If DateFrom and TimeFrom has a value, TimeTo must also be set.");
        
        RuleFor(x => x.TimeTo)
            .GreaterThan(x => x.TimeFrom)
            .When(x => x.Day.HasValue && x.TimeFrom.HasValue && x.TimeTo.HasValue)
            .WithMessage("TimeFrom cannot be less than TimeTo.");

        RuleFor(x => x)
            .Must(x => (x.TypeId.HasValue || x.ResourceId.HasValue) &&
            (
                (x.Day.HasValue && x.TimeFrom.HasValue && x.TimeTo.HasValue) ||
                (!x.Day.HasValue && !x.TimeFrom.HasValue && !x.TimeTo.HasValue)
            ));
    }
}
