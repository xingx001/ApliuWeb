using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApliuTools.Web
{
    public class ResponseMessage
    {
        public string code;
        public string result;
        public string msg;
        public string remark;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}