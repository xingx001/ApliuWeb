﻿using ApliuTools;
using ApliuTools.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ApliuWeb.Controllers
{
    public class ToolApiController : ApiController
    {
        [HttpGet]
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

        ///https://www.apliu.xyz/api/toolapi/sendsms?mobile=18779182730&smscontent=您正在使用短信服务，短信验证码是ACBXDFF，2分钟之内有效，如非本人操作，请忽略本短信。&smsappid=1400075540&smsappkey=b0a0f4466492c96fcd3d1d334cc01749
        [HttpPost]
        public string SendSMS(string Mobile, string SMSContent, string TcSMSAppId, string TcSMSAppKey)
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            string SendMsg = "发生异常";
            SMSMessage sms = new TencentSMS();
            bool sendresult = sms.SendSMS(Mobile, SMSContent, out SendMsg, TcSMSAppId, TcSMSAppKey);
            if (sendresult)
            {
                result.code = "0";
                result.result = "发送成功";
            }
            result.msg = SendMsg;
            return result.ToString();
        }

        [HttpGet]
        public string SendSMSGet(string Mobile, string SMSContent, string TcSMSAppId, string TcSMSAppKey)
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            string SendMsg = "发生异常";
            SMSMessage sms = new TencentSMS();
            bool sendresult = sms.SendSMS(Mobile, SMSContent, out SendMsg, TcSMSAppId, TcSMSAppKey);
            if (sendresult)
            {
                result.code = "0";
                result.result = "发送成功";
            }
            result.msg = SendMsg;
            return result.ToString();
        }

        [HttpPost]
        public string SendSMSCode()
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            string Mobile = HttpContextRequest.Form["Mobile"];
            CodeType codeType = (CodeType)HttpContextRequest.Form["codeType"].ToInt();

            if (codeType == CodeType.Register && !UserInfo.UserCheck(Mobile))
            {
                result.msg = "该用户已注册";
                return result.ToString();
            }

            if ((codeType == CodeType.Login || codeType == CodeType.ChangePassword) && UserInfo.UserCheck(Mobile))
            {
                result.msg = "该用户未注册";
                return result.ToString();
            }

            string SendMsg = "发生异常";
            bool sendresult = VerificationCode.SendSMS(Mobile, codeType, out SendMsg);
            if (sendresult)
            {
                result.code = "0";
                result.result = "发送成功";
            }
            else
            {
                result.code = "1";
                result.result = "发送失败";
            }
            result.msg = SendMsg;
            return result.ToString();
        }

        [HttpPost]
        public string ApliuAjax(object value)
        {
            return "{\"errorCode\":\"-1\",\"errorMsg\":\"默认方法\",\"value\":\"" + value + "\"}";
        }

        /// <summary>
        /// 获取临时文本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetTempContent()
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            string Key = System.Web.HttpContext.Current.Request.QueryString["Key"];
            if (!string.IsNullOrEmpty(Key) && Key.Trim().Length >= 100)
            {
                result.msg = "标识符Key（text/key）长度必须小于100";
                result.result = "执行失败";
                return result.ToString();
            }
            string sqlWhere = string.IsNullOrEmpty(Key) ? " and TextKey is null " : " and TextKey= '" + SecurityHelper.UrlDecode(Key.Trim(), Encoding.UTF8) + "' ";
            string userid = UserInfo.GetUserInfo().UserId;
            sqlWhere += string.IsNullOrEmpty(userid) ? " and UserId = 'Everyone' " : " and UserId= '" + userid + "' ";

            DataSet dsText = DataAccess.Instance.GetData("select top 1 TEXTCONTENT,UPDATETIME from TempText where 1=1 " + sqlWhere);
            if (dsText != null && dsText.Tables.Count > 0)
            {
                result.code = "0";
                string content = string.Empty;
                string datetime = string.Empty;
                if (dsText.Tables[0].Rows.Count > 0)
                {
                    content = SecurityHelper.UrlDecode(dsText.Tables[0].Rows[0]["TEXTCONTENT"].ToString(), Encoding.UTF8);
                    datetime = dsText.Tables[0].Rows[0]["UPDATETIME"].ToString();
                }
                result.msg = content;
                result.result = "查询成功";
                result.remark = datetime;
            }
            return result.ToString();
        }

        /// <summary>
        /// 保存临时文本
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SetTempContent()
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            string Content = System.Web.HttpContext.Current.Request.Form["Content"];
            string Key = System.Web.HttpContext.Current.Request.Form["Key"];//不存在key的时候，Key值为null
            if (!string.IsNullOrEmpty(Key) && Key.Trim().Length >= 100)
            {
                result.msg = "标识符Key（text/key）长度必须小于100";
                result.result = "执行失败";
                return result.ToString();
            }
            string sqlWhere = string.IsNullOrEmpty(Key) ? " and TextKey is null " : " and TextKey='" + SecurityHelper.UrlEncode(Key.Trim(), Encoding.UTF8) + "' ";
            string userid = string.IsNullOrEmpty(UserInfo.GetUserInfo().UserId) ? "Everyone" : UserInfo.GetUserInfo().UserId;
            sqlWhere += " and UserId = '" + userid + "'";

            string updatesql = string.Empty;
            DataSet dsText = DataAccess.Instance.GetData("select top 1 TEXTCONTENT from TempText where 1=1 " + sqlWhere);
            if (dsText != null && dsText.Tables.Count > 0 && dsText.Tables[0].Rows.Count > 0)
            {
                updatesql = string.Format(@"update TempText set UserId='{0}',TextContent='{1}',UpdateTime='{2}',IP='{3}' where 1=1 {4} ", userid, SecurityHelper.UrlEncode(Content, Encoding.UTF8), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), HYRequest.GetIP(), sqlWhere);
            }
            else
            {
                string guid = Guid.NewGuid().ToString().ToUpper();
                updatesql = string.Format(@"insert into TempText(TempId,UserId,TextContent,UpdateTime,IP,TextKey) values('{0}','{1}','{2}','{3}','{4}',{5}) ", guid, userid, SecurityHelper.UrlEncode(Content, Encoding.UTF8), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), HYRequest.GetIP(), string.IsNullOrEmpty(Key) ? "null" : ("'" + Key + "'"));
            }

            bool setResult = DataAccess.Instance.PostData(updatesql);
            if (setResult)
            {
                result.code = "0";
                result.msg = "更新成功";
                result.result = "更新成功";
            }
            return result.ToString();
        }

        /// <summary>
        /// 字符串处理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Security()
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            string type = System.Web.HttpContext.Current.Request.QueryString["type"];
            string content = System.Web.HttpContext.Current.Request.QueryString["content"];
            if (!type.Equals("GUID") && (String.IsNullOrEmpty(type) || String.IsNullOrEmpty(content)))
            {
                result.msg = "处理类型或内容不能为空";
                result.result = "执行失败";
                return result.ToString();
            }

            String srcContent = String.Empty;
            switch (type)
            {
                case "MD5": srcContent = SecurityHelper.MD5Encrypt(content, Encoding.UTF8);
                    break;
                case "GUID": srcContent = Guid.NewGuid().ToString().ToUpper();
                    break;
                case "ToUpper": srcContent = content.ToString().ToUpper();
                    break;
                case "ToLower": srcContent = content.ToString().ToLower();
                    break;
                case "UrlDecode": srcContent = SecurityHelper.UrlDecode(content, Encoding.UTF8);
                    break;
                case "UrlEncode": srcContent = SecurityHelper.UrlEncode(content, Encoding.UTF8);
                    break;
                case "ASCIIEncode":
                    byte[] array = System.Text.Encoding.UTF8.GetBytes(content);
                    for (int i = 0; i < array.Length; i++)
                    {
                        int asciicode = (int)(array[i]);
                        if (!String.IsNullOrEmpty(srcContent)) srcContent += "-";
                        srcContent += Convert.ToString(asciicode);
                    }
                    break;
                case "ASCIIDecode":
                    if (content.ToByte() != 0) srcContent = System.Text.Encoding.UTF8.GetString(new Byte[] { content.ToByte() });
                    else srcContent = "ASCII码 格式有误，只能单个码值转单字符";
                    break;
                default: break;
            }

            result.code = "0";
            result.msg = srcContent;
            result.result = "处理成功";

            return result.ToString();
        }

        /// <summary>
        /// 进制转换
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string BaseConver()
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            Int32 from = System.Web.HttpContext.Current.Request.QueryString["from"].ToString().ToInt();
            Int32 to = System.Web.HttpContext.Current.Request.QueryString["to"].ToString().ToInt();
            string content = System.Web.HttpContext.Current.Request.QueryString["content"];
            if (from <= 0 || to <= 0 || String.IsNullOrEmpty(content))
            {
                result.msg = "进制类型或内容不能为空";
                result.result = "执行失败";
                return result.ToString();
            }

            String srcContent = String.Empty;
            try
            {
                Int32 temp10 = Convert.ToInt32(content, from);
                srcContent = Convert.ToString(temp10, to).ToUpper();

                result.code = "0";
                result.msg = srcContent;
                result.result = "处理成功";
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
                result.result = "处理失败";
            }

            return result.ToString();
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
