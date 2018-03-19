using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace ApliuTools
{
    public class Logger
    {
        private static readonly string logPath = "~/Log/";
        private static SemaphoreSlim sthread = new SemaphoreSlim(1);

        /// <summary>
        /// 日记记录 Web应用
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteLogWeb(string Msg)
        {
            string filename = HttpContext.Current.Server.MapPath(logPath + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            try
            {
                sthread.Wait();

                using (StreamWriter sw = new StreamWriter(filename, true))
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
            string filename = System.IO.Directory.GetCurrentDirectory() + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            try
            {
                sthread.Wait();

                using (StreamWriter sw = new StreamWriter(filename, true))
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