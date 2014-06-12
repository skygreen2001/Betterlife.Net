using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Util.Common;
using System.Web.SessionState;

namespace AdminManage.HttpData
{
    /// <summary>
    /// 生成后台的图形验证码
    /// </summary>
    public class ValidateCode : IHttpHandler,IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int NumCount = 4;//预设产生4位数
                //取得亂數
                string str_ValidateCode = UtilNumber.RandomNumber(NumCount);
                /*用於驗證的Session*/
                context.Session["ValidateNumber"] = str_ValidateCode;

                //取得圖片物件
                System.Drawing.Image image = UtilImage.CreateCheckCodeImage(str_ValidateCode);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                /*輸出圖片*/
                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(ms.ToArray());
                ms.Close();
            }
            catch (Exception ex){
                Console.WriteLine("发生异常:"+ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}