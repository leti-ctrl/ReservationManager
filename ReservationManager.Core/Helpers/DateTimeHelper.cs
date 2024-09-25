namespace ReservationManager.Core.Validators
{
    public static class DateTimeHelper
    {
        public static TimeOnly? StringToTimeOnly(this string time)
        {
            if (TimeOnly.TryParse(time, out var timeOnly))
                return timeOnly;
            else
                return null;
        }
    }
}
