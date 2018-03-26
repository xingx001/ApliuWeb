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

        // GET api/toolapi/GetQRCode?Content=content
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

        [HttpGet]
        public string ExecuteSql(string Sql, string Type)
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";
            result.result = "执行失败";

            Sql = Sql.Trim();
            if (string.IsNullOrEmpty(Sql)) return result.ToString();
            switch (Type)
            {
                case "Get":
                    DataSet sqlds = DataAccess.Instance.GetData(Sql);
                    if (sqlds != null && sqlds.Tables.Count > 0)
                    {
                        result.code = "0";
                        result.msg = JsonConvert.SerializeObject(sqlds.Tables[0]);
                        result.result = "执行成功";
                    }
                    break;
                case "Post":
                    int rank = DataAccess.Instance.PostDataInt(Sql, null);
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
