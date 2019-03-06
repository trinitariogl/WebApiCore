using System;
using System.Collections.Generic;
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
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        //Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Translations))]
        //[Display(Name = "Email", ResourceType = typeof(Translations))]
        public string Email { get; set; }

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
