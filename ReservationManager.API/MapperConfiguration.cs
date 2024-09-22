using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.API
{
    public class MapperConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserDto>()
                .Map(d => d.Role, s => s.Type);


        }
    }
}
