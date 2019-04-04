

namespace CrossCutting.Utils.ExceptionService
{
    using System;

    public class ErrorException: Exception
    {
        public ErrorException(string message) : base(message)
        {
        }
    }
}
