using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;

namespace ApliuTools
{
    public class Logger
    {
        private static readonly string logPath = "Log/";
        private static SemaphoreSlim sthread = new SemaphoreSlim(1);

        private static string _RootDirectory = String.Empty;
        /// <summary>
        /// 程序跟目录 需要先初始化 C:/LogPath/
        /// </summary>
        /// </summary>
        public static string RootDirectory
        {
            get
            {
                if (String.IsNullOrEmpty(_RootDirectory))
                {
                    throw new Exception("程序跟目录未初始化");
                }
                else return _RootDirectory;
            }
            set { _RootDirectory = value; }
        }
        /// <summary>
        /// 使用初始化的程序跟目录写日志
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteLog(string Msg)
        {
            try
            {
                sthread.Wait();
                string filePath = RootDirectory + logPath;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string fileName = filePath + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                using (StreamWriter sw = new StreamWriter(fileName, true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " : " + Msg);
                    sw.Flush();
                    sw.Close();
                }
            }
            finally
            {
                sthread.Release();
            }
        }

        /// <summary>
        /// 日记记录 Web应用
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteLogWeb(string Msg)
        {
            try
            {
                sthread.Wait();
                string filePath = HttpContext.Current.Server.MapPath("~/" + logPath);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string fileName = filePath + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                using (StreamWriter sw = new StreamWriter(fileName, true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " : " + Msg);
                    sw.Flush();
                    sw.Close();
                }
            }
            finally
            {
                sthread.Release();
            }
        }

        /// <summary>
        /// 日志记录 桌面应用
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteLogDesktop(string Msg)
        {
            try
            {
                sthread.Wait();
                //string filename = System.IO.Directory.GetCurrentDirectory() +"/"+ logPath + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                Assembly assem = Assembly.GetExecutingAssembly();
                string assemDir = Path.GetDirectoryName(assem.Location);
                string filePath = Path.Combine(assemDir, logPath);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string fileName = filePath + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " : " + Msg);
                    sw.Flush();
                    sw.Close();
                }
            }
            finally
            {
                sthread.Release();
            }
        }
    }
}