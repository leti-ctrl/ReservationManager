using ReservationManager.DomainModel.Meta;

namespace Tests.EntityGenerators;

public class ReservationTypeInvalidModelGenerator : TheoryData<ReservationType>
{
    public ReservationTypeInvalidModelGenerator()
    {
        Add(new ReservationType()
        {
            Code = "",
            Name = "",
            Start = new TimeOnly(15, 00, 00),
            End = new TimeOnly(16, 00, 00)
        });
        Add(new ReservationType()
        {
            Code = null,
            Name = "TestName",
            Start = new TimeOnly(15, 00, 00),
            End = new TimeOnly(16, 00, 00)
        });
        Add(new ReservationType()
        {
            Code = "TestCode",
            Name = null,
            Start = new TimeOnly(15, 00, 00),
            End = new TimeOnly(16, 00, 00)
        });
        Add(new ReservationType()
        {
            Code = "TestCode",
            Name = "TestName",
            Start = new TimeOnly(18, 00, 00),
            End = new TimeOnly(16, 00, 00)
        });
        Add(new ReservationType()
        {
            Code = "TestCode",
            Name = "TestName",
            Start = new TimeOnly(),
            End = new TimeOnly()
        });
    }
}