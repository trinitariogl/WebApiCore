
namespace CrossCutting.Utils.ExceptionService
{
    using System;

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
