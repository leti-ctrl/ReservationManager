using ReservationManager.DomainModel.Base;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.DomainModel.Operation
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();    
    }
}
