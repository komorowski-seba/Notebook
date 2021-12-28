using System;

namespace WebApi.Common.Exceptions
{
    public class SecurityKeysException : Exception
    {
        public SecurityKeysException(string message) : base(message)
        {
        }
    }
}