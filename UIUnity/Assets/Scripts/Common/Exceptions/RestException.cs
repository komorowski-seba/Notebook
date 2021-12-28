using System;

namespace Common.Exceptions
{
    public class RestException : Exception
    {
        public RestException(string message) : base(message) { }
    }
}