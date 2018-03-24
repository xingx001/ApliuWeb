using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApliuWeb
{
    public class SiteConfig
    {
        private static SiteConfig config = new SiteConfig();

        public static SiteConfig Instance
        {
            get
            {
                //如果当前配置文件的修改时间小于上次的修改时间，则认为配置文件已经被修改，需重新加载
                if (configFileModifyDate < System.IO.File.GetLastWriteTime(configFilePath))
                    LoadConfig();

                return config;
            }
        }

        private readonly static string configFilePath = System.Web.HttpContext.Current.Server.MapPath("~/Config/BaseConfig.config");
        private static DateTime configFileModifyDate = DateTime.Now; //配置文件最后一次修改时间

        #region ==属性字段==
        private string _Domain;
        /// <summary>
        /// 网站域名
        /// </summary>
        public string Domain
        {
            get
            {
                if (_Domain.EndsWith("/"))
                    return _Domain;

                return _Domain + "/";
            }
            set { _Domain = value; }
        }

        private string _DatabaseConnection;
        /// <summary>
        /// SQL Server数据库链接字符串
        /// </summary>
        public string DatabaseConnection
        {
            get { return _DatabaseConnection; }
            set { _DatabaseConnection = value; }
        }

        private string _DatabaseType;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DatabaseType
        {
            get { return _DatabaseType; }
            set { _DatabaseType = value; }
        }

        private string _AppId;
        /// <summary>
        /// 腾讯云账号的APPID
        /// </summary>
        public string AppId
        {
            get { return _AppId; }
            set { _AppId = value; }
        }

        private string _SecretId;
        /// <summary>
        /// 腾讯云API密钥上申请的标识身份的 SecretId
        /// </summary>
        public string SecretId
        {
            get { return _SecretId; }
            set { _SecretId = value; }
        }

        private string _SecretKey;
        /// <summary>
        /// SecretId 对应唯一的 SecretKey , 而 SecretKey 会用来生成请求签名 Signature
        /// </summary>
        public string SecretKey
        {
            get { return _SecretKey; }
            set { _SecretKey = value; }
        }
        #endregion

        /// <summary>
        /// 加载配置信息
        /// </summary>
        public static void LoadConfig()
        {
            config = new SiteConfig();
            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);

            XmlNode node = null;
            node = doc.SelectSingleNode("//configuration/Domain");
            if (node != null)
            {
                config.Domain = node.InnerText.Trim();
            }

            string[] dbtype = new string[2] { "0", "SqlServerPath" };
            node = doc.SelectSingleNode("//configuration/DatabaseType");
            if (node != null)
            {
                dbtype = node.InnerText.Trim().Split('-');
                if (dbtype.Length != 2) config.DatabaseType = "0";
                else config.DatabaseType = dbtype[0];
            }

            if (dbtype.Length != 2) node = doc.SelectSingleNode("//configuration/" + "SqlServerPath");
            else node = doc.SelectSingleNode("//configuration/" + dbtype[1]);
            if (node != null)
            {
                config.DatabaseConnection = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/appSettings/AppId");
            if (node != null)
            {
                config.AppId = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/appSettings/SecretId");
            if (node != null)
            {
                config.SecretId = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/appSettings/SecretKey");
            if (node != null)
            {
                config.SecretKey = node.InnerText.Trim();
            }
            configFileModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 获取base.config文件appSettings中指定节点的值
        /// </summary>
        /// <param name="NodeName"></param>
        /// <returns></returns>
        public static string GetConfigNodeValue(string NodeName)
        {
            string value = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);

            XmlNode node = null;
            node = doc.SelectSingleNode("//configuration/appSettings/" + NodeName);
            if (node != null)
            {
                value = node.InnerText.Trim();
            }
            return value;
        }
    }
}
