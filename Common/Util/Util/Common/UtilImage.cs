using System;
using System.Drawing;

namespace Util.Common
{
    /// <summary>
    /// 图像处理相关工具类
    /// </summary>
    public static class UtilImage
    {
        /// <summary>
        /// 生成图形验证码
        /// </summary>
        /// <see cref="http://www.dotblogs.com.tw/shadow/archive/2011/10/05/38823.aspx"/>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public static System.Drawing.Image CreateCheckCodeImage(string checkCode,int height=28)
        {
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((checkCode.Length * 14), height);//产生图片，宽20*位数，高40像素
            System.Drawing.Graphics g = Graphics.FromImage(image);

            //生成随机数生成器
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int int_Red = 0;
            int int_Green = 0;
            int int_Blue = 0;
            int_Red = random.Next(256);//产生0~255
            int_Green = random.Next(256);//产生0~255
            int_Blue = (int_Red + int_Green > 400 ? 0 : 400 - int_Red - int_Green);
            int_Blue = (int_Blue > 255 ? 255 : int_Blue);

            //清空图片背景色
            //g.Clear(Color.FromArgb(int_Red, int_Green, int_Blue));
            g.Clear(Color.Black);

            //画图片的背景噪音线条
            //for (int i = 0; i <= 24; i++)
            //{
            //    int x1 = random.Next(image.Width);
            //    int x2 = random.Next(image.Width);
            //    int y1 = random.Next(image.Height);
            //    int y2 = random.Next(image.Height);

            //    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);

            //    g.DrawEllipse(new Pen(Color.DarkViolet), new System.Drawing.Rectangle(x1, y1, x2, y2));
            //}

            //Font font = new System.Drawing.Font("Arial", 20, (System.Drawing.FontStyle.Bold));
            Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Regular));
            //System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);
            Brush brush = new SolidBrush(Color.White);  
            g.DrawString(checkCode, font, brush, 7, 5);
            //for (int i = 0; i <= 99; i++)
            //{
            //    //画图片的前景噪音点
            //    int x = random.Next(image.Width);
            //    int y = random.Next(image.Height);

            //    image.SetPixel(x, y, Color.FromArgb(random.Next()));
            //}

            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            return image;
        }

    }
}
