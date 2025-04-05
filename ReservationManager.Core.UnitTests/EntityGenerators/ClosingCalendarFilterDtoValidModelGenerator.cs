using ReservationManager.Core.Dtos;

namespace Tests.EntityGenerators;

public class ClosingCalendarFilterDtoValidModelGenerator : TheoryData<ClosingCalendarFilterDto>
{
    public ClosingCalendarFilterDtoValidModelGenerator()
    {
        Add(new ClosingCalendarFilterDto
        {
            StartDay = null, 
            EndDay =  null
        });
        Add(new ClosingCalendarFilterDto 
        {
            StartDay = new DateOnly(2020, 01, 01), 
            EndDay =  null
            
        });
        Add(new ClosingCalendarFilterDto 
        {
            StartDay = new DateOnly(2020, 01, 01), 
            EndDay =  new DateOnly(2020, 01, 01)
        });
        Add(new ClosingCalendarFilterDto 
        {
            StartDay = new DateOnly(2020, 01, 01), 
            EndDay = new DateOnly(2021, 01, 01)
        });
    }
};