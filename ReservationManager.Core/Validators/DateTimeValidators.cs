namespace ReservationManager.Core.Validators
{
    public static class DateTimeValidators
    {
        public static TimeOnly? TimeOnlyValidator(this string time)
        {
            if (TimeOnly.TryParse(time, out var timeOnly))
                return timeOnly;
            else
                return null;
        }
    }
}
