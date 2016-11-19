using System;
using System.Collections.Generic;
using System.Threading;
using AkkaSample.Messages;
using AkkaSample.Nodes;
using Akka.Actor;
using AkkaSample.Helpers;

namespace AkkaSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var nodes = new List<IDisposable>();
            try
            {
                var server1 = new AkkaServer(8888);

                var client1 = new AkkaClient();

                //hold for the cluster to be running
                Thread.Sleep(2000);

                for (var i = 0; i < 1000; ++i)
                {
                    client1.TargetServer.Tell(new Stuff { HandlerId = i, Text = "Message " + i });
                }

                var server2 = new AkkaServer(8889);

                //hold for the cluster to be running
                Thread.Sleep(2000);

                for (var i = 0; i < 1000; ++i)
                {
                    client1.TargetServer.Tell(new Stuff { HandlerId = i, Text = "Message " + i });
                }

                ConsoleHelper.Write("Actor system running, press enter to exit...");
                Console.ReadLine();
                ConsoleHelper.Write("Bye!");
            }
            finally
            {
                foreach (var node in nodes)
                {
                    node.Dispose();
                }
            }
        }
    }
}
