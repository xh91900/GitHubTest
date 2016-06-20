using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Infrastructure.WCF
{
    internal class ServiceRealProxy<T>: RealProxy
    {
        private string configurationPath;

        private string endpointName;

        public ServiceRealProxy(string configurationPath, string endpointName) : base(typeof(T))
        {
            if (string.IsNullOrEmpty(endpointName))
            {
                throw new ArgumentNullException("endpointName");
            }
            this.endpointName = endpointName;
            this.configurationPath = configurationPath;
        }

        public override IMessage Invoke(IMessage msg)
        {
            T t = ChannelFactoryCreator.Create<T>(this.configurationPath, this.endpointName).CreateChannel();
            IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
            IMethodReturnMessage methodReturnMessage = null;
            object[] array = Array.CreateInstance(typeof(object), methodCallMessage.Args.Length) as object[];
            methodCallMessage.Args.CopyTo(array, 0);
            this.GetParameters(methodCallMessage);
            ServiceProxyFactory.GetEndpointAddress<T>(this.configurationPath, this.endpointName);
            try
            {
                object ret = methodCallMessage.MethodBase.Invoke(t, array);
                int outArgsCount = (array == null) ? 0 : array.Length;
                methodReturnMessage = new ReturnMessage(ret, array, outArgsCount, methodCallMessage.LogicalCallContext, methodCallMessage);
                object arg_9B_0 = methodReturnMessage.ReturnValue;
                (t as ICommunicationObject).Close();
            }
            catch (CommunicationException e)
            {
                (t as ICommunicationObject).Abort();
                methodReturnMessage = new ReturnMessage(e, methodCallMessage);
            }
            catch (TimeoutException e2)
            {
                (t as ICommunicationObject).Abort();
                methodReturnMessage = new ReturnMessage(e2, methodCallMessage);
            }
            catch (Exception e3)
            {
                (t as ICommunicationObject).Abort();
                methodReturnMessage = new ReturnMessage(e3, methodCallMessage);
            }
            return methodReturnMessage;
        }

        private Dictionary<string, object> GetParameters(IMethodCallMessage mcm)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            for (int i = 0; i < mcm.InArgCount; i++)
            {
                dictionary.Add(mcm.GetInArgName(i), mcm.GetInArg(i));
            }
            return dictionary;
        }
    }
}
