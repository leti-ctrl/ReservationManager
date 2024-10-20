using ReservationManager.DomainModel.Base;

namespace ReservationManager.Persistence.Interfaces.Base
{
    public interface ICrudEditableTypeRepository<T> : ICrudBaseTypeRepository<T> where T : EditableType 
    {
        
        
    }
}
