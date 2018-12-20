

namespace CrossCutting.Utils.ExceptionService
{
    using System;

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
