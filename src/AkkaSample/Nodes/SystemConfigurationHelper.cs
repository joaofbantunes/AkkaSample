using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Configuration;

namespace AkkaSample.Nodes
{
    class SystemConfigurationHelper
    {
        public const string ClusterName = "TestAkkaCluster";

        private const string SystemConfigString = @"
                akka {  
                    actor {
                        provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
                    }
                    remote {
#log-remote-lifecycle-events = DEBUG
                        helios.tcp {
                            port = PORT
                            hostname = 127.0.0.1
                            public-hostname = localhost
                        }
                    }
                    cluster {
                            seed-nodes = [""akka.tcp://TestAkkaCluster@localhost:8888""]
                            roles = [""ROLE""]
                            auto-down-unreachable-after = 10s
                    }
                }";


        internal static Config GetConfig(int port, string role)
        {
            var systemConfigString = SystemConfigString.Replace("PORT", port.ToString()).Replace("ROLE", role);
            return ConfigurationFactory.ParseString(systemConfigString);
        }
    }
}
