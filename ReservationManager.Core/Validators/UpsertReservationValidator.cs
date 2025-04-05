using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Validators;

public class UpsertReservationValidator : IUpsertReservationValidator
{

    public bool IsDateRangeValid(UpsertReservationDto reservation, ReservationType rezType)
    {
        if (reservation.Day < DateOnly.FromDateTime(DateTime.Now))
            return false;
        
        if (rezType.Code == FixedReservationType.Customizable)
            return reservation.Start.HasValue && reservation.End.HasValue && reservation.Start < reservation.End;

        if (rezType.Code != FixedReservationType.Customizable)
            return !reservation.Start.HasValue && !reservation.End.HasValue ;

        return false;
    }

}