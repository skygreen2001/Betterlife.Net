using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Util.Common
{
    public class UtilFtpFileUpload
    {
        private string ftpHost = "ftp://210.51.51.164:213";
        private string httpHost = "http://210.51.51.164:89";
        private string username = "administrator";
        private string password = "5ikhABCD";
        private bool IsUpload2Local=true;
        private string UrlWebsite = "http://localhost:9999/";
        private string UploadUrl = "http://localhost:9999/Uploads";
        private string UploadPath = "C:\\\\net\\Betterlife.Net\\Admin\\Uploads";

        public UtilFtpFileUpload()
        {
            init();
        }

        //初始化
        public void init()
        {

        }
        
        //上传图片
        public Dictionary<string, string> upload(string savePath, Stream inputStream, int size)
        {
            return IsUpload2Local ? upload2Local(savePath, inputStream, size) : upload2Ftp(savePath, inputStream, size);
        }

        // 登陆
        private FtpWebRequest makeLogin(string username, string password, string url)
        {
            FtpWebRequest reqObj = (FtpWebRequest)WebRequest.Create(url);
            reqObj.Credentials = new NetworkCredential(username, password);
            return reqObj;
        }

        //上传本地流到ftp
        private Dictionary<string,string> upload2Ftp(string savePath, Stream inputStream, int size)
        {
            checkFtpDir(savePath);

            Dictionary<string, string> ret = new Dictionary<string, string>();

            FtpWebRequest req = null;
            try
            {
                req = makeLogin(username, password, ftpHost + savePath);
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.ContentLength = size;
                const int buffer_size = 2048;
                byte[] buffer = new byte[buffer_size];
                int dataRead = 0;

                using (inputStream)
                {
                    using (Stream output = req.GetRequestStream())
                    {
                        do
                        {
                            dataRead = inputStream.Read(buffer, 0, buffer_size);
                            output.Write(buffer, 0, dataRead);
                        } while (!(dataRead < buffer_size));
                        output.Close();
                    }
                }

            }
            catch (Exception e)
            {

                ret.Add("success", false.ToString());
                ret.Add("error", e.Message);
                return ret;
            }
            finally
            {
                inputStream.Close();
                req.Abort();
                req = null;
            }

            ret.Add("success", true.ToString());
            ret.Add("domain", httpHost);
            ret.Add("full_path", httpHost + "/uploads" + savePath);
            ret.Add("img_path", "/uploads" + savePath);
            return ret;
             
        }

        //上传到本地
        private Dictionary<string, string> upload2Local(string savePath, Stream inputStream, int size)
        {

            string localPath = UploadPath + savePath.Replace("/", @"\");

            Dictionary<string, string> ret = new Dictionary<string, string>();

            try
            {
                checkLocalDir(localPath);

                byte[] uploadFileBytes = new byte[size];
                inputStream.Read(uploadFileBytes, 0, size);

                File.WriteAllBytes(localPath, uploadFileBytes);
            }
            catch (Exception e)
            {
                ret.Add("success", false.ToString());
                ret.Add("error", e.Message);
                return ret;
            }
            finally 
            {
                inputStream.Close();
            }

            ret.Add("success", true.ToString());
            ret.Add("domain", UrlWebsite);
            ret.Add("full_path", UploadUrl + savePath);
            ret.Add("img_path", "/uploads" + savePath);
            return ret; 
        }

        //创建目录
        private Boolean makeDir(string localFile)
        {
            FtpWebRequest req = null;
            try
            {
                //string FtpPath = "ftp://210.51.51.164:213/cc/mm/sdf";
                req = makeLogin(username, password, ftpHost + localFile);
                //req = makeLogin(Username, Password, FtpUrl + localFile);

                req.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }

        private void checkLocalDir(string dir)
        {
            dir = getLocalPath(dir);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        //检查路径不存在则创建
        private void checkFtpDir(string fullDir)
        {
            fullDir = getFtpPath(fullDir);
            string[] dirs = fullDir.Split('/');
            string curDir = "";
            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];
                //如果是以/开始的路径,第一个为空  
                if (dir != null && dir.Length > 0)
                {
                    try
                    {
                        curDir += "/" + dir;
                        makeDir(curDir);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        // 根据文件全路径 得到目录全路径
        private static string getFtpPath(string destFilePath)
        {
            return destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
        }

        private static string getLocalPath(string destFilePath)
        {
            return destFilePath.Substring(0, destFilePath.LastIndexOf("\\"));
        }
    }
}
