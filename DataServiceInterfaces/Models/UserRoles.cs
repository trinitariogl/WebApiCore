namespace DataServiceInterfaces.Models
{
    public class UserRoles
    {
        public string RoleId { get; set; }
        public virtual Roles Rol { get; set; }

        public string UserId { get; set; }
        public virtual UserAccounts User { get; set; }
    }
}
