using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Infrastructure.WCF
{
    internal static class ChannelFactoryCreator
    {
        private static Hashtable channelFactories = new Hashtable();

        public static ChannelFactory<T> Create<T>(string configurationPath, string endpointName)
        {
            if (string.IsNullOrEmpty(endpointName))
            {
                throw new ArgumentNullException("endpointName");
            }
            ChannelFactory<T> channelFactory = null;
            if (ChannelFactoryCreator.channelFactories.ContainsKey(endpointName))
            {
                channelFactory = (ChannelFactoryCreator.channelFactories[endpointName] as ChannelFactory<T>);
            }
            if (channelFactory == null)
            {
                channelFactory = new CustomClientChannel<T>(endpointName, configurationPath);
                lock (ChannelFactoryCreator.channelFactories.SyncRoot)
                {
                    ChannelFactoryCreator.channelFactories[endpointName] = channelFactory;
                }
            }
            return channelFactory;
        }
    }
}
