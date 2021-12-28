using System;
using Common.Interface;

namespace Common.Messages
{
    public class ViewMessage : IMessage
    {
        public Type Receiver { get; set; }
        public IView Sender { get; set; }
        public ViewMessageEnum Message { get; set; }
    }
    
    public enum ViewMessageEnum
    {
        Show, Hide
    }
}