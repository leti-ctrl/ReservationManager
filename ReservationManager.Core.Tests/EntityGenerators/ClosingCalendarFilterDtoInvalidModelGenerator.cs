using ReservationManager.Core.Dtos;

namespace Tests.EntityGenerators;

public class ClosingCalendarFilterDtoInvalidModelGenerator : TheoryData<ClosingCalendarFilterDto>
{
    public ClosingCalendarFilterDtoInvalidModelGenerator()
    {
        Add(new ClosingCalendarFilterDto
        {
            StartDay = null, 
            EndDay = new DateOnly(2020, 01, 01)
        });
        Add(new ClosingCalendarFilterDto
        {
            StartDay = new DateOnly(2021, 01, 01), 
            EndDay = new DateOnly(2020, 01, 01)
        });
    }
};