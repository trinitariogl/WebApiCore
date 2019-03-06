using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServiceInterfaces.Models
{
    [Serializable]
    public class UserAccountDto
    {
        /*[Required]
        [StringLength(128, ErrorMessageResourceName = "CannotExceed", ErrorMessageResourceType = typeof(Translations))]
        [Display(Name = "Id", ResourceType = typeof(Translations))]*/
        public string Id { get; set; }

        /*[Required]
        [StringLength(0, ErrorMessageResourceName = "CannotExceed", ErrorMessageResourceType = typeof(Translations))]
        [Display(Name = "Username", ResourceType = typeof(Translations))]*/
        public string Username { get; set; }

        /*[Required]
        [StringLength(80, ErrorMessageResourceName = "CannotExceed", ErrorMessageResourceType = typeof(Translations))]
        [Display(Name = "Email", ResourceType = typeof(Translations))]*/
        public string Email { get; set; }

        public string Password { get; set; }

        /*[Required]
        [StringLength(8, ErrorMessageResourceName = "CannotExceed", ErrorMessageResourceType = typeof(Translations))]
        [Display(Name = "PrefferedLanguage", ResourceType = typeof(Translations))]*/
        public string PrefferedLanguage { get; set; }

        /*[Required]
        [Display(Name = "PasswordHash", ResourceType = typeof(Translations))]*/
        public byte[] PasswordHash { get; set; }

        /*[Required]
        [Display(Name = "Salt", ResourceType = typeof(Translations))]*/
        public byte[] Salt { get; set; }

        /*[Required]
        [Display(Name = "Active", ResourceType = typeof(Translations))]*/
        public bool Active { get; set; }

        /*[Required]
        [Display(Name = "VerificationToken", ResourceType = typeof(Translations))]*/
        public Nullable<System.Guid> VerificationToken { get; set; }

        public ICollection<RoleDto> UserRoles { get; set; }
    }
}
