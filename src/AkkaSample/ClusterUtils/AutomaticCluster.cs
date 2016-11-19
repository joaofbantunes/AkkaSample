//based on https://github.com/akkadotnet/akka.net/blob/dev/src/examples/Cluster/ClusterSharding/ClusterSharding.Node/AutomaticJoin/AutomaticCluster.cs

using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Akka.Cluster;
using System.Collections.Generic;

namespace AkkaSample.ClusterUtils
{

    public class AutomaticCluster
    {
        private static readonly IList<Address> Nodes;

        private readonly ActorSystem _system;
        private readonly Cluster _cluster;

        static AutomaticCluster()
        {
            Nodes = new List<Address>();
        }
        public AutomaticCluster(ActorSystem system)
        {
            _system = system;
            _cluster = Cluster.Get(system);
        }

        public void Join()
        {
            var members = Nodes.ToImmutableList();
            if (members.Any())
            {
                _cluster.JoinSeedNodes(members);
                Nodes.Add(_cluster.SelfAddress);
            }
            else
            {
                var self = _cluster.SelfAddress;
                Nodes.Add(self);
                _cluster.JoinSeedNodes(ImmutableList.Create(self));
            }
        }

        public void Leave()
        {
            Nodes.Remove(_cluster.SelfAddress);
        }
    }
}