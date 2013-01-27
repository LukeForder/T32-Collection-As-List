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
    }
}
