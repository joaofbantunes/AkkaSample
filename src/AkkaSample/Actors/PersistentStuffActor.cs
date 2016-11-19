using AkkaSample.Helpers;
using AkkaSample.Messages;
using Akka.Persistence;
using System.Collections.Generic;
using Akka.Cluster;
using System;
using Akka.Actor;
using Akka.Cluster.Sharding;

namespace AkkaSample.Actors
{
    public class PersistentStuffActor : ReceivePersistentActor
    {
        public override string PersistenceId { get; } = Context.Parent.Path.Name + "/" + Context.Self.Path.Name;

        private readonly string _address;

        private IList<Stuff> _messages = new List<Stuff>();
        private int _messageCount;

        public PersistentStuffActor()
        {
            _address = Cluster.Get(Context.System).SelfAddress.ToString();

            SetReceiveTimeout(TimeSpan.FromSeconds(3));

            Command<ReceiveTimeout>(receiveTimeout =>
            {
                ConsoleHelper.Write("Actor {0}/{1} timedout, sending passivate message to parent.", _address, PersistenceId);
                Context.Parent.Tell(new Passivate(PoisonPill.Instance));
            });

            Recover<Stuff>(HandleRecoverStuff);
            Command<Stuff>(HandlePersistNewStuff);
            Command<GetInfo>(HandleGetInfo);
            ConsoleHelper.Write("Actor {0}/{1} created", _address, PersistenceId);
        }

        private bool HandleRecoverStuff(Stuff stuff)
        {
            ++_messageCount;
            ConsoleHelper.Write("From persistence -> (Actor {0}/{1} received \"{2}\" - message count: {3})", _address, PersistenceId, stuff.Text, _messageCount);
            return true;
        }
        private bool HandlePersistNewStuff(Stuff stuff)
        {
            Persist(stuff, HandleNewStuff);
            return true;
        }
        private void HandleNewStuff(Stuff stuff)
        {
            ++_messageCount;
            ConsoleHelper.Write("New message -> (Actor {0}/{1} received \"{2}\" - message count: {3})", _address, PersistenceId, stuff.Text, _messageCount);
        }
        private bool HandleGetInfo(GetInfo getInfo)
        {
            Sender.Tell(string.Format("Actor {0}/{1} GetInfo result: {2} messages have been received.", _address, PersistenceId, _messageCount), Self);
            return true;
        }

    }
}
