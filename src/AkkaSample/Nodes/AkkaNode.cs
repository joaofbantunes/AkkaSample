using AkkaSample.Messages.Sharding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaSample.Nodes
{
    public abstract class AkkaNode
    {
        protected MessageExtractor GetMessageExtractor()
        {
            return new MessageExtractor(10);
        }
    }
}
