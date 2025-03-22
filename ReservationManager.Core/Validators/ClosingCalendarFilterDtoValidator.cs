using FluentValidation;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Validators
{
    public class ClosingCalendarFilterDtoValidator : AbstractValidator<ClosingCalendarFilterDto>, IClosingCalendarFilterDtoValidator
    {
        public ClosingCalendarFilterDtoValidator()
        {
            RuleFor(x => x.StartDay)
                .NotNull()
                .When(x => x.EndDay.HasValue)
                .WithMessage("You cannot set an end date without a start date.");
            
            RuleFor(x => x.StartDay)
                .LessThanOrEqualTo(x => x.EndDay)
                .When(x => x.StartDay.HasValue && x.EndDay.HasValue)
                .WithMessage("StartDay must be before or equal to EndDay.");
        }

    }
}
