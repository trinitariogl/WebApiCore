using System;

namespace DataServiceInterfaces.Models
{
    public class UserRoles
    {
        public Guid RoleId { get; set; }
        public virtual Roles Rol { get; set; }

        public Guid UserId { get; set; }
        public virtual UserAccounts User { get; set; }
    }
}
