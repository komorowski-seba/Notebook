using System;

namespace Common.Interface
{
    public interface IMessage 
    {
        Type Receiver { get; set; }
    }
}