using System;
using Common.Interface;
using UniRx;

namespace Controller
{
    public class BrokerMessage : IBrokerMessage
    {
        private readonly ISubject<IMessage> _messages = new Subject<IMessage>();

        public void Publish(IMessage message) => _messages.OnNext(message);
        public IObservable<TMsg> Receive<TMsg>() => _messages.OfType<IMessage, TMsg>();
    }
}