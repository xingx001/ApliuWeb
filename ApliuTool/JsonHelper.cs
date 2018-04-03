using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuTools
{
    public class JsonHelper
    {
        /// <summary>
        /// 将json转成url参数
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonConvertUrl(string json)
        {
            string urlparams = string.Empty;
            JArray jo = (JArray)JsonConvert.DeserializeObject(json);
            foreach (JToken child in jo)
            {
                JProperty jPro = child as JProperty;
                if (!string.IsNullOrEmpty(urlparams)) urlparams += "&";
                urlparams += jPro.Name + "=" + jPro.Value;
            }
            return urlparams;
        }
    }
}
