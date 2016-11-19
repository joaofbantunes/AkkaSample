using System;
using Akka.Actor;
using AkkaSample.ClusterUtils;
using Akka.Cluster.Sharding;
using AkkaSample.Messages.Sharding;

namespace AkkaSample.Nodes
{
    public class AkkaClient : AkkaNode, IDisposable
    {
        public const string Role = "client";

        private readonly ActorSystem _system;
        private readonly AutomaticCluster _automaticCluster;

        public IActorRef TargetServer { get; private set; }

        public AkkaClient()
        {
            _system = ActorSystem.Create(SystemConfigurationHelper.ClusterName, SystemConfigurationHelper.GetConfig(0, Role));
            _automaticCluster = new AutomaticCluster(_system);
            _automaticCluster.Join();
            TargetServer = ClusterSharding.Get(_system).StartProxy(
                typeName: "stuffer",
                role: null,
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
