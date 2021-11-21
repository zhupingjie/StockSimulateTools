using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StockSimulateCore.Utils
{
    public class DownFileUtil
    {
        public static string ReplaceSpecialCharacters(string fileName)
        {
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)//GetInvalidPathChars()
            {
                string invalid = new string(Path.GetInvalidFileNameChars());
                foreach (char c in invalid)
                {
                    fileName = fileName.Replace(c.ToString(), "_");
                }
            }
            return fileName;
        }

        /// 下载文件方法
        /// 文件保存路径和文件名
        /// 返回服务器文件名
        public static string DownloadFile(string sourceFile, string desPath, string fileName)
        {
            bool flag = false;
            long SPosition = 0;
            FileStream FStream = null;
            Stream myStream = null;

            var desFile = Path.Combine(desPath, ReplaceSpecialCharacters(fileName));

            try
            {
                //判断要下载的文件夹是否存在
                if (File.Exists(desFile))
                {
                    //打开上次下载的文件
                    FStream = File.OpenWrite(desFile);
                    //获取已经下载的长度
                    SPosition = FStream.Length;
                    long serverFileLength = GetHttpLength(sourceFile);
                    if (SPosition == serverFileLength)
                    {//文件是完整的，直接结束下载任务
                        return null;
                    }
                    FStream.Seek(SPosition, SeekOrigin.Current);
                }
                else
                {
                    //文件不保存创建一个文件
                    FStream = new FileStream(desFile, FileMode.Create);
                    SPosition = 0;
                }
                //打开网络连接
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(sourceFile);
                if (SPosition > 0)
                {
                    myRequest.AddRange(SPosition);             //设置Range值
                }
                //向服务器请求,获得服务器的回应数据流
                myStream = myRequest.GetResponse().GetResponseStream();
                //定义一个字节数据
                byte[] btContent = new byte[512];
                int intSize = 0;
                intSize = myStream.Read(btContent, 0, 512);
                while (intSize > 0)
                {
                    FStream.Write(btContent, 0, intSize);
                    intSize = myStream.Read(btContent, 0, 512);
                }
                flag = true;        //返回true下载成功
            }
            catch (Exception ex)
            {
                throw new Exception($"文件下载错误:{ex.Message}");
            }
            finally
            {
                //关闭流
                if (myStream != null)
                {
                    myStream.Close();
                    myStream.Dispose();
                }
                if (FStream != null)
                {
                    FStream.Close();
                    FStream.Dispose();
                }
            }
            return desFile;
        }
        static long GetHttpLength(string url)
        {
            long length = 0;
            try
            {
                var req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                req.Method = "HEAD";
                req.Timeout = 5000;
                var res = (HttpWebResponse)req.GetResponse();
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    length = res.ContentLength;
                }
                res.Close();
                return length;
            }
            catch (WebException wex)
            {
                return 0;
            }
        }

        #region 移动文件后删掉指定文件
        public static void MoveTempFile(string OrignFile, string NewFile)
        {
            //覆盖模式
            if (File.Exists(NewFile))
            {
                File.Delete(NewFile);
            }
            if (File.Exists(OrignFile))
            {
                File.Move(OrignFile, NewFile);
                File.Delete(OrignFile);
            }
        }
        #endregion

        public static string DownloadFile2(string url, string fileName)
        {
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;

                System.IO.Stream st = myrp.GetResponseStream();
                var desFile = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}", "downfile", "pdf");
                if (!Directory.Exists(desFile)) Directory.CreateDirectory(desFile);
                var fileUrl = desFile + $@"\{fileName}";
                System.IO.Stream so = new System.IO.FileStream(fileUrl, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);
                }
                //System.Threading.Thread.Sleep(60 * 1000);
                so.Close();
                st.Close();
                so.Dispose();
                st.Dispose();
                return fileUrl;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }


   
}
