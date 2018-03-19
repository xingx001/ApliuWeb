using ApliuTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApliuWeb.Controllers
{
    public class ToolApiController : ApiController
    {
        // GET api/tool
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/tool/5
        public HttpResponseMessage GetQRCode(string id)
        {
            return QRCode.CreateCodeSimple(id);
        }

        // POST api/tool
        public void Post([FromBody]string value)
        {
        }

        // PUT api/tool/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/tool/5
        public void Delete(int id)
        {
        }
    }
}
