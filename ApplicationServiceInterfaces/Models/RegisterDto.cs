using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationServiceInterfaces.Models
{
    public class RegisterDto
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>        
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Translations))]
        //[Display(Name = "Username", ResourceType = typeof(Translations))]
        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        //Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Translations))]
        //[Display(Name = "Email", ResourceType = typeof(Translations))]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Translations))]
        //[Display(Name = "Role", ResourceType = typeof(Translations))]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Translations))]
        //[Display(Name = "Language", ResourceType = typeof(Translations))]
        public string Language { get; set; }
    }
}
