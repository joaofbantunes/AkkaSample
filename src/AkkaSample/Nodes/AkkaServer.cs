using System;
using Akka.Actor;
using Akka.Cluster.Sharding;
using AkkaSample.Actors;
using AkkaSample.ClusterUtils;
using AkkaSample.Messages.Sharding;

namespace AkkaSample.Nodes
{
    public class AkkaServer : AkkaNode, IDisposable
    {
        public const string Role = "server";

        private readonly ActorSystem _system;
        private readonly AutomaticCluster _automaticCluster;


        public AkkaServer(int port)
        {
            _system = ActorSystem.Create(SystemConfigurationHelper.ClusterName, SystemConfigurationHelper.GetConfig(port, Role));
            _automaticCluster = new AutomaticCluster(_system);
            _automaticCluster.Join();

            var sharding = ClusterSharding.Get(_system);
            var shardRegion = sharding.Start(
                typeName: "stuffer",
                entityProps: Props.Create<PersistentStuffActor>(),
                settings: ClusterShardingSettings.Create(_system).WithRole(Role),
                messageExtractor: GetMessageExtractor());
        }

        public void Dispose()
        {
            _automaticCluster.Leave();
            _system.Terminate();
            _system.Dispose();
        }
    }
}
