using FluentValidation;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Interfaces.Validators;

public interface IUpsertReservationValidator 
{
    bool IsDateRangeValid(UpsertReservationDto reservation, ReservationType? rezType);
}