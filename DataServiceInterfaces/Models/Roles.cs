
namespace DataServiceInterfaces.Models
{
    using CrossCutting.Core;
    using System.Collections.Generic;

    public partial class Roles: Entity
    {
        public Roles()
        {
            this.UserRoles = new HashSet<UserRoles>();
        }

        //public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public override bool IsValid => true;
    }
}
