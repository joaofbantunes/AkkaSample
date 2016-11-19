using Akka.Cluster.Tools.Singleton;
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
                            #seed-nodes = [""akka.tcp://TestAkkaCluster@localhost:8888""]
                            roles = [""ROLE""]
                            auto-down-unreachable-after = 10s
                            sharding {
                                least-shard-allocation-strategy.rebalance-threshold = 2
                            }
                    }
                    persistence {
                        journal {
                            plugin = ""akka.persistence.journal.sqlite""
                            sqlite {
                                class = ""Akka.Persistence.Sqlite.Journal.SqliteJournal, Akka.Persistence.Sqlite""
                                connection-string = ""Data Source=.\\store.db""
                                auto-initialize = true
                            }
                        }
                    
                        snapshot-store {
                            plugin = ""akka.persistence.snapshot-store.sqlite""
                            sqlite {
                              class = ""Akka.Persistence.Sqlite.Snapshot.SqliteSnapshotStore, Akka.Persistence.Sqlite""
                              connection-string = ""Data Source=.\\store.db""
                              auto-initialize = true
                            }
                        }
                    }
                    #log-dead-letters-during-shutdown = on
                }";


        internal static Config GetConfig(int port, string role)
        {
            var systemConfigString = SystemConfigString.Replace("PORT", port.ToString()).Replace("ROLE", role);
            return ConfigurationFactory.ParseString(systemConfigString).WithFallback(ClusterSingletonManager.DefaultConfig());
        }
    }
}
