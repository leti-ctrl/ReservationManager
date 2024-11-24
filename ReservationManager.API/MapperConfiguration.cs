using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.API
{
    public class MapperConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UpsertReservationTypeDto, ReservationType>()
                .Map(d => d.Start, s => s.StartTime)
                .Map(d => d.End, s => s.EndTime)
                .Map(d => d.Code, s => s.Code.ToUpperInvariant());
            
            
        }
    }
}
