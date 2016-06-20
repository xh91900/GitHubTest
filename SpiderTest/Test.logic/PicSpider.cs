using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Common;
using Test.Model;

namespace Test.logic
{
    public class PicSpider
    {
        HttpHelper httphelper = new HttpHelper();

        public string SpiderDo(string targetUrl,string referer,string token)
        {
            httphelper.Token = token;
            return httphelper.HttpGet(targetUrl, referer, Encoding.UTF8, false, false, 50000);
        }

        public void SpiderDo(string targetUrl, string referer)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(p => { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetJson"></param>
        /// <returns></returns>
        public M500PX ConvertJson(string targetJson)
        {
            M500PX model=JsonConvert.DeserializeObject<M500PX>(targetJson);
            return model;
        }

        public void DelImage()
        {
            ImageHelper helper = new ImageHelper();
            FileStream stream = new FileStream("F:\\pictures\\temp1.jpg", FileMode.Open, FileAccess.Read);
            byte[] Byte= null;

            const int bufferLenth = 1024;
            int actual = 0;
            byte[] buffer = new byte[bufferLenth];
            while ((actual = stream.Read(buffer, 0, bufferLenth)) > 0)
            {
                stream.Read(buffer, 0, actual);
            }
            Byte = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(Byte, 0, Byte.Length);

            if (Byte.Length / 1024 >100)
            {
                Byte = helper.PictureCompression(Byte, 60 - (Byte.Length / 10240));
            }
            FileStream stream1 = new FileStream("F:\\pictures\\temp7.jpg", FileMode.OpenOrCreate, FileAccess.Write);
            stream1.Write(Byte, 0, Byte.Length);
            stream.Dispose();
            stream.Close();
            stream1.Dispose();
            stream1.Close();
            helper.GetPicThumbnail("F:\\pictures\\temp0.jpg", "F:\\pictures\\temp2.jpg", 720, 100);
        }
    }
}
