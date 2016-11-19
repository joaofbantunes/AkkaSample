using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaSample.Helpers;
using AkkaSample.Messages;

namespace AkkaSample.Actors
{
    public class StuffRouterActor : ReceiveActor
    {
        private readonly Guid _id;
        private readonly IDictionary<int, IActorRef> _children;

        public StuffRouterActor()
        {
            _id = Guid.NewGuid();
            _children = new Dictionary<int, IActorRef>();
            ReceiveAsync<Stuff>(HandleStuff);
        }

        private Task HandleStuff(Stuff message)
        {
            IActorRef child;
            if (!_children.TryGetValue(message.HandlerId, out child))
            {
                child = Context.ActorOf(Props.Create(() => new StuffActor(message.HandlerId)), "stuffer" + message.HandlerId);
                _children.Add(message.HandlerId, child);
            }
            ConsoleHelper.Write("Passed through router: {0}",_id);
            child.Tell(message);
            return Task.FromResult(0);
        }
    }
}
