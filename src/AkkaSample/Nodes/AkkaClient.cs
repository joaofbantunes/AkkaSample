using System;
using Akka.Actor;
using Akka.Cluster.Routing;
using Akka.Routing;
using AkkaSample.Actors;

namespace AkkaSample.Nodes
{
    public class AkkaClient : IDisposable
    {
        public const string Role = "client";

        private readonly ActorSystem _system;

        public IActorRef TargetServer { get; private set; }

        public AkkaClient()
        {
            _system = ActorSystem.Create(SystemConfigurationHelper.ClusterName, SystemConfigurationHelper.GetConfig(0, Role));
            TargetServer = _system.ActorOf(Props.Create<StuffRouterActor>().WithRouter(new ClusterRouterPool(new ConsistentHashingPool(4), new ClusterRouterPoolSettings(4, 2, false, AkkaServer.Role))), "serverRouter");
        }

        public void Dispose()
        {
            _system.Dispose();
        }
    }
}
