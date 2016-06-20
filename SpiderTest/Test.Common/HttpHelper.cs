using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.Common
{
    /// <summary>
    /// HttpHelper
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Cookie
        /// </summary>
        private CookieContainer cc;

        public string AcceptEncoding { get; set; }

        public string Accept { get; set; }

        public string AcceptLanguage { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        private int index = 0;

        /// <summary>
        /// 获取或设置一个 System.Boolean 值，该值确定是否使用 100-Continue 行为。
        /// </summary>
        public bool Expect100Continue { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否与 Internet 资源建立持久性连接。
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        public HttpHelper()
        {
            this.index = this.GetRandom(0, 3);
            this.cc = new CookieContainer();
        }

        /// <summary>
        /// 使用get方式访问目标网页，返回html页面
        /// </summary>
        /// <param name="targetURL">url</param>
        /// <param name="referer">referer</param>
        /// <param name="encoding">encoding</param>
        /// <param name="requestClientIp">是否使用IP欺骗</param>
        /// <param name="isUseProxyIP">是否使用代理IP</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>html</returns>
        public string HttpGet(string targetUrl,string referer,Encoding encoding,bool requestClientIp, bool isUseProxyIP, int timeout)
        {
            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetUrl);
            request.CookieContainer = cc;
            request.Method = "GET";
            request.Referer = referer;
            request.Timeout = timeout;
            if (!string.IsNullOrEmpty(AcceptEncoding))
            {
                request.Headers["Accept-Encoding"] = AcceptEncoding;
                request.AutomaticDecompression = DecompressionMethods.GZip;
            }

            if (string.IsNullOrEmpty(Accept) == false)
            {
                request.Accept = Accept;
            }

            if (string.IsNullOrEmpty(AcceptLanguage) == false)
            {
                request.Headers.Add("Accept-Language:" + AcceptLanguage);
            }

            // 暂时500px用一用
            if(!string.IsNullOrEmpty(Token))
            {
                request.Headers.Add("X-CSRF-Token:" + Token);
            }

            bool isHttps = targetUrl.ToLower().Contains("https");
            if (isHttps)
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

            }

            //BGetProxyIP bGetProxyIP = null;
            if (requestClientIp)
            {
                request.Headers.Add("X-Forwarded-For", this.GetRequestIP());
                request.Headers.Add("Client-Ip", this.GetRequestIP());
            }
            if (KeepAlive)
            {
                request.KeepAlive = KeepAlive;
            }

            request.ServicePoint.Expect100Continue = Expect100Continue;

            if (UserAgent != null)
            {
                request.UserAgent = UserAgent;

            }
            else
            {
                request.UserAgent = this.GetUseAgent();
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    cc.Add(response.Cookies);
                    using (Stream stream = response.GetResponseStream())
                    {
                        html = new StreamReader(stream, encoding).ReadToEnd();
                        return html;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 使用get方式访问目标图片
        /// </summary>
        /// <param name="targetURL">url</param>
        /// <param name="referer">referer</param>
        /// <param name="encoding">encoding</param>
        /// <param name="requestClientIp">是否使用IP欺骗</param>
        /// <param name="isUseProxyIP">是否使用代理IP</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>byte[]</returns>
        public byte[] HttpImageGet(string targetUrl,string referer,bool requestClientIp, bool isUseProxyIP, int timeout)
        {
            byte[] imgByte = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetUrl);
            request.CookieContainer = cc;
            request.Method = "GET";
            request.Referer = referer;
            request.Timeout = timeout;
            if (!string.IsNullOrEmpty(AcceptEncoding))
            {
                request.Headers["Accept-Encoding"] = AcceptEncoding;
                request.AutomaticDecompression = DecompressionMethods.GZip;
            }

            if (requestClientIp)
            {
                request.Headers.Add("X-Forwarded-For", this.GetRequestIP());
                request.Headers.Add("Client-Ip", this.GetRequestIP());
            }

            // 暂时500px用一用
            if (!string.IsNullOrEmpty(Token))
            {
                request.Headers.Add("X-CSRF-Token:" + Token);
            }

            request.UserAgent = this.GetUseAgent();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    cc.Add(response.Cookies);
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            const int bufferLenth = 1024;
                            int actual = 0;
                            byte[] buffer = new byte[bufferLenth];
                            while ((actual = stream.Read(buffer, 0, bufferLenth)) > 0)
                            {
                                ms.Write(buffer, 0, actual);
                            }
                            imgByte = new byte[ms.Length];
                            ms.Position = 0;
                            ms.Read(imgByte, 0, imgByte.Length);

                            return imgByte;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取一个请求IP（随机生成C类IP地址：192.168.0.0到192.168.255.255 ）
        /// </summary>
        /// <returns>C类IP地址</returns>
        public string GetRequestIP()
        {
            string first = this.GetRandom(0, 255).ToString();
            string second = this.GetRandom(0, 255).ToString();
            string third = GetRandom(0, 255).ToString();
            string fourth = GetRandom(0, 255).ToString();

            return string.Format("{0}.{1}.{2}.{3}", first, second, third, fourth);
        }

        /// <summary>
        /// 获得一个随机数
        /// </summary>
        /// <param name="minValue">随机数最小值</param>
        /// <param name="maxValue">随机数最大值</param>
        /// <returns>随机数</returns>
        private int GetRandom(int minValue, int maxValue)
        {
            try
            {

                // 生成随机种子  用于随机返回代理IP 
                // int seekSeek = unchecked((int)DateTime.Now.Ticks);
                Random random = new Random(Chaos_GetRandomSeed());
                return random.Next(minValue, maxValue);
            }
            catch
            {
                return minValue;
            }
        }

        /// <summary>
        /// 加密随机数生成器，生成随机种子
        /// </summary>
        /// <returns>随机种子</returns>
        public static int Chaos_GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 获取 UseAgent
        /// </summary>
        /// <returns></returns>
        public string GetUseAgent()
        {
            List<string> list = new List<string>();
            list.Add("Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)");
            list.Add("Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 2.0.1124)");
            list.Add("Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");

            return list[index];
        }
    }
}
