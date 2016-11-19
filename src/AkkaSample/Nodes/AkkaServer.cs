using System;
using Akka.Actor;

namespace AkkaSample.Nodes
{
    public class AkkaServer : IDisposable
    {
        public const string Role = "server";

        private readonly ActorSystem _system;
        

        public AkkaServer(int port)
        {
            _system = ActorSystem.Create(SystemConfigurationHelper.ClusterName, SystemConfigurationHelper.GetConfig(port, Role));
        }

        public void Dispose()
        {
            _system.Dispose();
        }
    }
}
