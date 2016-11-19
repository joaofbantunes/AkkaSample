using AkkaSample.Helpers;
using AkkaSample.Messages;
using System.Collections.Generic;
using Akka.Cluster;
using System;
using Akka.Actor;
using Akka.Cluster.Sharding;
using System.Threading.Tasks;
using AkkaSample.Persistence;

namespace AkkaSample.Actors
{
    public class StuffActor : ReceiveActor
    {
        public int Id { get; } = int.Parse(Context.Self.Path.Name);
        public string PersistenceId { get; } = Context.Parent.Path.Name + "/" + Context.Self.Path.Name;

        private readonly string _address;

        private IList<Stuff> _messages = new List<Stuff>();
        private int _messageCount;

        public StuffActor()
        {
            _address = Cluster.Get(Context.System).SelfAddress.ToString();

            SetReceiveTimeout(TimeSpan.FromSeconds(3));
            Receive<ReceiveTimeout>(receiveTimeout =>
            {
                ConsoleHelper.Write("Actor {0}/{1} timedout, sending passivate message to parent.", _address, PersistenceId);
                Context.Parent.Tell(new Passivate(PoisonPill.Instance));
            });

            ReceiveAsync<Stuff>(HandleStuffAsync);
            ReceiveAsync<GetInfo>(HandleGetInfoAsync);
            ConsoleHelper.Write("Actor {0}/{1} created", _address, PersistenceId);
            _messageCount = StaticStore.Get(Id);
        }

        private async Task HandleStuffAsync(Stuff stuff)
        {
            ++_messageCount;
            StaticStore.Set(Id, _messageCount);
            await ConsoleHelper.WriteAsync("New message -> (Actor {0}/{1} received \"{2}\" - message count: {3})", _address, PersistenceId, stuff.Text, _messageCount);
        }
        private Task HandleGetInfoAsync(GetInfo getInfo)
        {
            Sender.Tell(string.Format("Actor {0}/{1} GetInfo result: {2} messages have been received.", _address, PersistenceId, _messageCount), Self);
            return Task.CompletedTask;
        }

    }
}
