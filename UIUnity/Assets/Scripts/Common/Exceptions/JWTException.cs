using System;

namespace Common.Exceptions
{
    public class JWTException : Exception
    {
        public JWTException(string message) : base(message) { }
    }
}