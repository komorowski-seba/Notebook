using System;
using UniRx;

namespace Common.Interface
{
    public interface IBrokerMessage
    {
        IObservable<TMessage> Receive<TMessage>();
        void Publish(IMessage message);
    }
}