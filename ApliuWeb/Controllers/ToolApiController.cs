﻿using ApliuTools;
using ApliuTools.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApliuWeb.Controllers
{
    public class ToolApiController : ApiController
    {
        [HttpHead]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// GET api/toolapi/GetQRCode?Content=content
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetQRCode(string Content)
        {
            return QRCode.CreateCodeSimple(Content);
        }

        // GET api/toolapi/SearchTool?Keyword=content
        [HttpGet]
        public HttpResponseMessage SearchTool(string Keyword)
        {
            return HttpRequestHelper.encapResult(Keyword);
        }

        [HttpGet]
        public HttpResponseMessage SearchSubmit(string sid, string keyword)
        {
            return HttpRequestHelper.encapResult(sid + keyword);
        }

        /// <summary>
        /// 链接服务器的测试数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        [HttpGet]
        public string ExecuteSql(string Sql, string Type)
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            Sql = Sql.Trim();
            if (string.IsNullOrEmpty(Sql)) return result.ToString();

            string databaseType = string.Empty;
            string databaseConnection = string.Empty;
            SiteConfig.GetDatabaseConfig("DatabaseTest", out databaseType, out databaseConnection);
            DataAccess.LoadDataAccess("DatabaseTest", databaseType, databaseConnection);
            switch (Type.ToUpper())
            {
                case "GET":
                    DataSet sqlds = DataAccess.InstanceKey["DatabaseTest"].GetData(Sql);
                    if (sqlds != null && sqlds.Tables.Count > 0)
                    {
                        result.code = "0";
                        result.msg = JsonConvert.SerializeObject(sqlds.Tables[0]);
                        result.result = "执行成功";
                    }
                    break;
                case "POST":
                    int rank = DataAccess.InstanceKey["DatabaseTest"].PostDataInt(Sql, null);
                    if (rank >= 0)
                    {
                        result.code = "0";
                        result.msg = "受影响的数据条数：" + rank;
                        result.result = "执行成功";
                    }
                    break;
                default:
                    break;
            }
            return result.ToString();
        }

        /// <summary>
        /// 链接指定数据库进行操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        [HttpGet]
        public string ExecuteDatabseSql(string source, string userid, string password, string database, string sql)
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            sql = sql.Trim();
            if (string.IsNullOrEmpty(sql)) return result.ToString();

            string conn = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=False;User ID={2};Password={3};Connect Timeout=15;Encrypt=False;TrustServerCertificate=False", source, database, userid, password);

            string databaseType = "0";
            string databaseConnection = conn;
            string ip = HYRequest.GetIP();//以客户端IP作为Key，避免重复加载数据库链接对象
            string key = SecurityHelper.MD5Encrypt(ip, System.Text.Encoding.UTF8);
            if (string.IsNullOrEmpty(ip)) key = Guid.NewGuid().ToString().Replace("-", "");
            DataAccess.LoadDataAccess(key, databaseType, databaseConnection);
            if (sql.ToUpper().Contains("UPDATE") || sql.ToUpper().Contains("DELETE") || sql.ToUpper().Contains("INSERT"))
            {
                int rank = DataAccess.InstanceKey[key].PostDataInt(sql, null);
                if (rank >= 0)
                {
                    result.code = "0";
                    result.msg = "受影响的数据条数：" + rank;
                    result.result = "执行成功";
                }
            }
            else
            {
                DataSet sqlds = DataAccess.InstanceKey[key].GetData(sql);
                if (sqlds != null && sqlds.Tables.Count > 0)
                {
                    result.code = "0";
                    result.msg = JsonConvert.SerializeObject(sqlds.Tables[0]);
                    result.result = "执行成功";
                }
            }
            return result.ToString();
        }

        [HttpPost]
        public string SendSMS(string Mobile, string SMSContent, string SMSAppId, string SMSAppKey)
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            string SendMsg = "发生异常";
            SMSMessage sms = new TencentSMS();
            sms.SendSMS(Mobile, SMSContent, out SendMsg, SMSAppId, SMSAppKey);
            result.msg = SendMsg;
            return result.ToString();
        }

        [HttpPost]
        public string ApliuAjax(object value)
        {
            return "{\"errorCode\":\"-1\",\"errorMsg\":\"默认方法\"}";
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
