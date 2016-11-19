using System;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaSample.Helpers;
using AkkaSample.Messages;

namespace AkkaSample.Actors
{
    public class StuffActor : ReceiveActor
    {
        private readonly int _id;

        public StuffActor(int id)
        {
            _id = id;
            ReceiveAsync<Stuff>(stuff =>
            {
                ConsoleHelper.Write("Actor {0} received \"{1}\"", _id, stuff.Text);
                return Task.FromResult(0);
            });
            ConsoleHelper.Write("Actor {0} created", _id);
        }
    }
}
