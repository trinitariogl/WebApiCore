

namespace ApplicationServiceInterfaces.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represent response for security operations
    /// </summary>
    [Serializable]
    public sealed class SecurityResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityResult"/> class.
        /// </summary>
        /// <param name="errors">The errors.</param>
        public SecurityResult(params string[] errors)
            : this((ICollection<string>)errors)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityResult"/> class.
        /// </summary>
        /// <param name="errors">The errors.</param>
        internal SecurityResult(ICollection<string> errors)
        {
            this.Errors = errors;
        }

        /// <summary>
        /// Gets the success.
        /// </summary>
        public static SecurityResult Success
        {
            get { return new SecurityResult(); }
        }

        /// <summary>
        /// Faileds the specified errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static SecurityResult Failed(params string[] errors)
        {
            if (errors.Count() == 0)
            {
                errors = new string[1]
                {
                    String.Empty
                    //Translations.DefaultError
                };
            }
            return new SecurityResult(errors);
        }

        /// <summary>
        /// Gets a value indicating whether [is success].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is success]; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess
        {
            get { return Errors.Count() == 0; }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public ICollection<string> Errors { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Empty;
            //return IsSuccess ? Translations.Success : Translations.Failure + ": " + string.Join(" - ", Errors.ToArray());
        }
    }
}
