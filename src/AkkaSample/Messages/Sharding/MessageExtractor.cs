using Akka.Cluster.Sharding;

namespace AkkaSample.Messages.Sharding
{
    public sealed class ShardEnvelope
    {
        public readonly int EntityId;
        public readonly object Payload;

        public ShardEnvelope(int entityId, object payload)
        {
            EntityId = entityId;
            Payload = payload;
        }
    }

    public sealed class MessageExtractor : HashCodeMessageExtractor
    {
        public MessageExtractor(int maxNumberOfShards) : base(maxNumberOfShards) { }

        public override string EntityId(object message) => (message as ShardEnvelope)?.EntityId.ToString();

        public override object EntityMessage(object message) => (message as ShardEnvelope)?.Payload;
    }
}
