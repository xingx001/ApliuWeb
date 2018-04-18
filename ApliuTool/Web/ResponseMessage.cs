﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApliuTools.Web
{
    public class ResponseMessage
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string code;
        /// <summary>
        /// 执行结果
        /// </summary>
        public string result;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}