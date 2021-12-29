using System;

namespace WebApi.Application.Exceptions
{
    public class SecurityKeysException : Exception
    {
        public SecurityKeysException(string message) : base(message)
        {
        }
    }
}