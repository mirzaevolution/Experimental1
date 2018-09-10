using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
namespace Experimental1.Controllers
{
    public class UploadHandlerController : ApiController
    {
        [Route("UploadHandler/Submit")]
        public IHttpActionResult Upload()
        {
            var files = HttpContext.Current.Request.Files;
            StringBuilder sb = new StringBuilder();
            if (files!=null)
            {
                int len = files.Count;
                for(int i = 0; i < len; i++)
                {
                    sb.AppendLine($"name=`{files[i].FileName}`; Content-Type=`{files[i].ContentType}`;");
                }
                return Ok(sb.ToString());
            }
            else
            {
                sb.AppendLine("No file(s) uploaded");
                return InternalServerError(new Exception(sb.ToString()));
            }

        }
    }
}