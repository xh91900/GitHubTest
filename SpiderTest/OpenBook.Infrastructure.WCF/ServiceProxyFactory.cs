using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Infrastructure.WCF
{
    /// <summary>
    /// 服务代理工厂
    /// </summary>
    public class ServiceProxyFactory
    {
        /// <summary>
        /// 创建代理服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurationPath">配置文件路径</param>
        /// <param name="endpointName">根节点名称</param>
        /// <returns>契约（T）</returns>
        public static T Create<T>(string configurationPath,string endpointName)
        {
            if(string.IsNullOrEmpty(endpointName))
            {
                throw new ArgumentException("endpointName");
            }
            return (T)((object)new ServiceRealProxy<T>(configurationPath, endpointName).GetTransparentProxy());
        }

        /// <summary>
        /// 获取根节点地址
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurationPath">配置文件路径</param>
        /// <param name="endpointName">根节点名称</param>
        /// <returns>string</returns>
        public static string GetEndpointAddress<T>(string configurationPath, string endpointName)
        {
            return ChannelFactoryCreator.Create<T>(configurationPath, endpointName).Endpoint.Address.Uri.ToString();
        }
    }
}
