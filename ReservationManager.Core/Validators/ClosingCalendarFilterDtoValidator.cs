using FluentValidation;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Validators
{
    public class ClosingCalendarFilterDtoValidator : AbstractValidator<ClosingCalendarFilterDto>, IClosingCalendarFilterDtoValidator
    {
        public ClosingCalendarFilterDtoValidator()
        {
            RuleFor(x => x)
                .Must(x => x.StartDay == null || x.EndDay == null || x.StartDay <= x.EndDay)
                .WithMessage("StartDay must be before or equal to EndDay.");
        }

    }
}
