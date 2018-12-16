
namespace DataServiceInterfaces.Models
{
    using System.Collections.Generic;

    public partial class Roles
    {
        public Roles()
        {
            this.UserRoles = new HashSet<UserRoles>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
