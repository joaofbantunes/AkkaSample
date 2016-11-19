using System;
using System.Collections.Generic;
using AkkaSample.Messages;
using AkkaSample.Nodes;
using Akka.Actor;
using AkkaSample.Helpers;
using AkkaSample.Messages.Sharding;
using System.IO;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AkkaSample
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncContext.Run(MainAsync);            
        }

        private static async Task MainAsync()
        {
            var nodes = new List<IDisposable>();
            try
            {
                await ConsoleHelper.WriteAsync("Removing sqlite file");
                File.Delete("store.db");

                await ConsoleHelper.WriteAsync("Starting...");

                await ConsoleHelper.WriteAsync("Adding server 1");
                var server1 = new AkkaServer(8888);
                nodes.Add(server1);

                ConsoleHelper.Write("Adding client 1");
                var client1 = new AkkaClient();
                nodes.Add(client1);

                await ConsoleHelper.WriteAsync("Holding for client 1 to get into cluster.");
                await Task.Delay(5000);

                var firstBatchOfMessagesCount = 10;
                ConsoleHelper.Write($"Sending first batch of {firstBatchOfMessagesCount} messages.");
                for (var i = 0; i < firstBatchOfMessagesCount; ++i)
                {
                    client1.TargetServer.Tell(new ShardEnvelope(i, new Stuff { Text = "Batch 1 - Message " + i }));
                }

                await ConsoleHelper.WriteAsync("Letting the first batch process.");
                await Task.Delay(4000);

                await ConsoleHelper.WriteAsync("Adding server 2");
                var server2 = new AkkaServer(8889);
                nodes.Add(server2);

                await ConsoleHelper.WriteAsync("Holding for server 2 to get into cluster.");
                await Task.Delay(2000);

                await ConsoleHelper.WriteAsync("Holding a bit more for actors to timeout.");
                await Task.Delay(4000);

                var secondBatchOfMessagesCount = 10;
                await ConsoleHelper.WriteAsync($"Sending second batch of {secondBatchOfMessagesCount} messages.");

                for (var i = 0; i < secondBatchOfMessagesCount; ++i)
                {
                    client1.TargetServer.Tell(new ShardEnvelope(i, new Stuff { Text = "Batch 2 - Message " + i }));
                }

                await Task.Delay(1000);
                await ConsoleHelper.WriteAsync("Keeping GetInfo results organized in the log.");
                var thirdBatchOfMessagesCount = 10;
                ConsoleHelper.Write($"Sending {thirdBatchOfMessagesCount} get info messages.");
                for (var i = 0; i < thirdBatchOfMessagesCount; ++i)
                {
                    var info = await client1.TargetServer.Ask<string>(new ShardEnvelope(i, new GetInfo()));
                    ConsoleHelper.Write(info);
                }

                ConsoleHelper.WriteNow("Bye!");
            }
            finally
            {
                foreach (var node in nodes)
                {
                    try
                    {
                        node.Dispose();
                    }
                    catch (Exception ex)
                    {
                        ConsoleHelper.Write("Failed to dispose of node");
                        ConsoleHelper.Write(ex.ToString());
                    }
                }
            }
        }
    }
}
