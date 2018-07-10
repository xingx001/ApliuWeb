﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace ApliuTools
{
    public class Logger
    {
        private static readonly string logPath = "Log/";
        private static SemaphoreSlim sthread = new SemaphoreSlim(1);

        /// <summary>
        /// 程序跟目录 需要先初始化
        /// </summary>
        private static string _RootDirectory = String.Empty;
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
                string filename = RootDirectory + logPath + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        /// 日记记录 Web应用
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteLogWeb(string Msg)
        {
            try
            {
                sthread.Wait();
                string filename = HttpContext.Current.Server.MapPath("~/" + logPath + DateTime.Now.ToString("yyyyMMdd") + ".txt");
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
            try
            {
                sthread.Wait();
                string filename = System.IO.Directory.GetCurrentDirectory() + logPath + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                using (StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8))
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