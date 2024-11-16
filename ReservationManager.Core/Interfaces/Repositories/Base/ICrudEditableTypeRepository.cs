using ReservationManager.DomainModel.Base;

namespace ReservationManager.Core.Interfaces.Repositories.Base
{
    public interface ICrudEditableTypeRepository<T> : ICrudBaseTypeRepository<T> where T : EditableType 
    {
        
        
    }
}
