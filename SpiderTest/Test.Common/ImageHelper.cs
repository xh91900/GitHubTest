using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Common
{
    /// <summary>
    /// ImageHelper
    /// </summary>
    public class ImageHelper
    {
        //public Image ResourceImage = Image.FromFile("");

        //public Image GetReducedImage(int width, int height)
        //{
        //    //用指定的大小和格式初始化Bitmap类的新实例
        //    Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

        //    //从指定的Image对象创建新Graphics对象
        //    Graphics graphics = Graphics.FromImage(bitmap);

        //    //在指定位置并且按指定大小绘制原图片对象
        //    graphics.DrawImage(ResourceImage, new Rectangle(0, 0, width, height));
        //    return bitmap;
        //}

        /// <summary>
        /// 截取图片方法
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="beginX">开始位置-X</param>
        /// <param name="beginY">开始位置-Y</param>
        /// <param name="getX">截取宽度</param>
        /// <param name="getY">截取长度</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="fileExt">后缀名</param>
        public static string CutImage(string url, int beginX, int beginY, int getX, int getY, string fileName, string savePath, string fileExt)
        {
            if ((beginX < getX) && (beginY < getY))
            {
                Bitmap bitmap = new Bitmap(url);//原图
                if (((beginX + getX) <= bitmap.Width) && ((beginY + getY) <= bitmap.Height))
                {
                    Bitmap destBitmap = new Bitmap(getX, getY);//目标图
                    Rectangle destRect = new Rectangle(0, 0, getX, getY);//矩形容器
                    Rectangle srcRect = new Rectangle(beginX, beginY, getX, getY);

                    Graphics.FromImage(destBitmap);
                    //Graphics.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);

                    ImageFormat format = ImageFormat.Png;
                    switch (fileExt.ToLower())
                    {
                        case "png":
                            format = ImageFormat.Png;
                            break;
                        case "bmp":
                            format = ImageFormat.Bmp;
                            break;
                        case "gif":
                            format = ImageFormat.Gif;
                            break;
                    }
                    destBitmap.Save(savePath + "//" + fileName, format);
                    return savePath + "\\" + "*" + fileName.Split('.')[0] + "." + fileExt;
                }
                else
                {
                    return "截取范围超出图片范围";
                }
            }
            else
            {
                return "请确认(beginX < getX)&&(beginY < getY)";
            }
        }

        #region GetPicThumbnail
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth"></param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>

        public bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dWidth"></param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>

        public bool GetPicThumbnail(string sFile, string dFile, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {
                sW = dWidth;

                sH = (int)((decimal)dWidth / (decimal)tem_size.Width * tem_size.Height);
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, sH);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle(-1, -1, sW + 1, sH + 1), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targerImg"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public byte[] PictureCompression(byte[] targerImg, int flag)
        {
            byte[] imgByte = null;
            MemoryStream result = new MemoryStream();
            MemoryStream ms = new MemoryStream();
            ms.Write(targerImg, 0, targerImg.Length);
            Bitmap ob = new Bitmap(ms, true);

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {

                    ob.Save(result, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(result, ImageFormat.Jpeg);
                }
                const int bufferLenth = 1024;
                int actual = 0;
                byte[] buffer = new byte[bufferLenth];
                while ((actual = result.Read(buffer, 0, bufferLenth)) > 0)
                {
                    result.Write(buffer, 0, actual);
                }
                imgByte = new byte[result.Length];
                result.Position = 0;
                result.Read(imgByte, 0, imgByte.Length);
                return imgByte;
            }
            catch
            {
                return null;
            }
            finally
            {
                ob.Dispose();
                ms.Dispose();
                ms.Close();
            }
        }
    }
}
