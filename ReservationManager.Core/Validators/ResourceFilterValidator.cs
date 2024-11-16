using FluentValidation;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Validators;

public class ResourceFilterValidator : AbstractValidator<ResourceFilterDto>, IResourceFilterValidator
{
    public ResourceFilterValidator()
    {
        // Rule for DateFrom, DateTo, TimeFrom, TimeTo:
        // They must either all have values or all be null
        RuleFor(x => x.DateFrom)
            .NotNull()
            .When(x => x.DateTo.HasValue || !string.IsNullOrEmpty(x.TimeFrom) || !string.IsNullOrEmpty(x.TimeTo))
            .WithMessage("If DateTo, TimeFrom, or TimeTo have a value, DateFrom must also be set.");

        RuleFor(x => x.DateTo)
            .NotNull()
            .When(x => x.DateFrom.HasValue)
            .WithMessage("If DateFrom has a value, DateTo must also be set.");

        RuleFor(x => x.TimeFrom)
            .NotNull()
            .When(x => x.DateFrom.HasValue)
            .WithMessage("If DateFrom has a value, TimeFrom must also be set.");

        RuleFor(x => x.TimeTo)
            .NotNull()
            .When(x => x.DateFrom.HasValue)
            .WithMessage("If DateFrom has a value, TimeTo must also be set.");

        // Rule to ensure that all date/time fields are either all null or all set
        RuleFor(x => x)
            .Must(x =>
                (x.DateFrom.HasValue && x.DateTo.HasValue && !string.IsNullOrEmpty(x.TimeFrom) && !string.IsNullOrEmpty(x.TimeTo)) ||
                (!x.DateFrom.HasValue && !x.DateTo.HasValue && string.IsNullOrEmpty(x.TimeFrom) && string.IsNullOrEmpty(x.TimeTo))
            )
            .WithMessage("DateFrom, DateTo, TimeFrom, and TimeTo must either all have values or all be null.");
    }
}
