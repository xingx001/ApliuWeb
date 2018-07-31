﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Web;

namespace ApliuTools.Web
{
    /// <summary>
    /// 服务器信息类
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// 服务器版本
        /// </summary>
        public string ServerOS
        {
            get
            {
                return Environment.OSVersion.ToString();
            }
        }

        /// <summary>
        /// 机器名称
        /// </summary>
        public string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        /// <summary>
        /// 站点绝对目录
        /// </summary>
        public string SitePath
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
            }
        }

        /// <summary>
        ///  DotNET 版本
        /// </summary>
        public string DotNetVersion
        {
            get
            {
                return ".NET CLR " + Environment.Version.ToString();
            }
        }

        public string SiteTotalFileSize
        {
            get
            {
                return (GetDirectorySize(this.SitePath) / 1024).ToString() + " MB";
            }
        }

        private long longDirSize = 0;
        /// <summary>
        /// 获取目录的大小
        /// </summary>
        /// <param name="srcPath">目录路径</param>
        /// <returns>目录的大小(单位:KB)</returns>
        public long GetDirectorySize(string srcPath)
        {
            try
            {

                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就重新调用GetDirSize(string srcPath)
                    if (System.IO.Directory.Exists(file))
                        GetDirectorySize(file);
                    else
                        longDirSize += GetFileSize(file);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return longDirSize / 1024;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="file">文件完整路径</param>
        /// <returns>文件大小(单位:bytes)</returns>
        public long GetFileSize(string file)
        {
            System.IO.FileInfo fiArr = new System.IO.FileInfo(file);
            return fiArr.Length;
        }

    }
}
