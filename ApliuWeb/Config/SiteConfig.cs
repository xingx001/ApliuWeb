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
        public string TcAppId
        {
            get { return _AppId; }
            set { _AppId = value; }
        }

        private string _SecretId;
        /// <summary>
        /// 腾讯云API密钥上申请的标识身份的 TcSecretId
        /// </summary>
        public string TcSecretId
        {
            get { return _SecretId; }
            set { _SecretId = value; }
        }

        private string _SecretKey;
        /// <summary>
        /// TcSecretId 对应唯一的 TcSecretKey , 而 TcSecretKey 会用来生成请求签名 Signature
        /// </summary>
        public string TcSecretKey
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

            node = doc.SelectSingleNode("//configuration/appSettings/TcAppId");
            if (node != null)
            {
                config.TcAppId = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/appSettings/TcSecretId");
            if (node != null)
            {
                config.TcSecretId = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("//configuration/appSettings/TcSecretKey");
            if (node != null)
            {
                config.TcSecretKey = node.InnerText.Trim();
            }
            configFileModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 获取指定链接库配置
        /// </summary>
        /// <param name="DatabaseTypeSelect"></param>
        public static void GetDatabaseConfig(string DatabaseTypeSelect, out string databaseType, out string databaseConnection)
        {
            databaseType = string.Empty;
            databaseConnection = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);

            XmlNode node = null;
            string[] dbtype = new string[] { };
            node = doc.SelectSingleNode("//configuration/" + DatabaseTypeSelect);
            if (node != null)
            {
                dbtype = node.InnerText.Trim().Split('-');
                if (dbtype.Length == 2) databaseType = dbtype[0];
            }

            if (dbtype.Length == 2)
            {
                node = doc.SelectSingleNode("//configuration/" + dbtype[1]);
                if (node != null)
                {
                    databaseConnection = node.InnerText.Trim();
                }
            }
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

            XmlNode node = doc.SelectSingleNode("//configuration/appSettings");

            XmlElement xElem = (XmlElement)node.SelectSingleNode("//add[@key='" + NodeName + "']");

            if (xElem != null)
            {
                value = xElem.GetAttribute("value");
            }
            return value;
        }
    }
}
