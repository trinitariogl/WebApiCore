

namespace DataServiceInterfaces.Models
{
    using CrossCutting.Core;
    using System;
    using System.Collections.Generic;

    public partial class UserAccounts : Entity
    {
        public UserAccounts()
        {
            this.UserRoles = new HashSet<UserRoles>();
        }

        //public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PrefferedLanguage { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public bool Active { get; set; }
        public Nullable<Guid> VerificationToken { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public override bool IsValid => true;

        public void Update(string userName, string email, byte[] pass)
        {
            this.Username = userName;
            this.Email = email;
            this.PasswordHash = pass;
        }

        public void UpdatePassword(byte[] pass)
        {
            this.PasswordHash = pass;
        }
    }
}
