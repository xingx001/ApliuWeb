using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace ApliuWeb
{
    public class Logger
    {
        private static readonly string logPath = "~/Log/";
        private static SemaphoreSlim sthread = new SemaphoreSlim(1);

        public static void WriteLog(string Msg)
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
    }
}