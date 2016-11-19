using Akka.Routing;

namespace AkkaSample.Messages
{
    public class Stuff : IConsistentHashable
    {
        public int HandlerId { get; set; }
        public string Text { get; set; }

        public object ConsistentHashKey
        {
            get { return HandlerId; }
        }
    }
}
