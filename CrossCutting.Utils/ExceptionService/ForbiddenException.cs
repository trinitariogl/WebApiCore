
namespace CrossCutting.Utils.ExceptionService
{
    using System;

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}
