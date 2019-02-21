

namespace ApplicationServiceInterfaces.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [Serializable]
    public class LogOnDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [Required]
        //[Display(Name = "LoginUser", ResourceType = typeof(Translations))]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        //[Display(Name = "Password", ResourceType = typeof(Translations))]
        public string Password { get; set; }
    }
}
