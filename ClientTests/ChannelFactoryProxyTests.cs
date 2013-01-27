using Contracts.Domain;
using Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace ClientTests
{
    public class ChannelFactoryProxyTests
    {
        [Fact]
        public void StandardChannelFactoryGeneratesProxyWithICollectionAsArray()
        {
            ChannelFactory<IPetShopService> channelFactory = new ChannelFactory<IPetShopService>("noExtensions");
            IPetShopService channel = channelFactory.CreateChannel();
            List<Cat> cats = channel.All();

            Assert.True(cats[0].Descendants is Cat[]);
        }

        [Fact]
        public void ChannelFactoryWithBehaviorGeneratesProxyWithICollectionAsList()
        {
            ChannelFactory<IPetShopService> channelFactory = new ChannelFactory<IPetShopService>("extensions");
            IPetShopService channel = channelFactory.CreateChannel();
            List<Cat> cats = channel.All();

            Assert.True(cats[0].Descendants is List<Cat>);
        }

        [Fact]
        public void DeserializationSupportsCyclicReferences()
        {
            ChannelFactory<IPetShopService> channelFactory = new ChannelFactory<IPetShopService>("extensions");
            IPetShopService channel = channelFactory.CreateChannel();

            // no stack over flow here
            new Action(() => channel.Cyclic()).ShouldNotThrow();           
        }

        [Fact]
        public void TransformationsAppliedToNestedMembers()
        {
            ChannelFactory<IPetShopService> channelFactory = new ChannelFactory<IPetShopService>("extensions");
            IPetShopService channel = channelFactory.CreateChannel();

            Queue<Cat> catQueue = new Queue<Cat>();
            catQueue.Enqueue(channel.NestedCats());

            while (catQueue.Count > 0)
            {
                Cat cat = catQueue.Dequeue();

                Assert.True(cat.Descendants is List<Cat>);

                foreach (Cat descendant in cat.Descendants)
                    catQueue.Enqueue(descendant);          
      
                
            }
        }
    }
}
