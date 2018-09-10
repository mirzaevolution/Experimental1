//https://stackoverflow.com/questions/40632028/asp-net-how-read-a-multipart-form-data-in-web-api

using Experimental1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
namespace Experimental1.Controllers
{
    public class UploadHandlerController : ApiController
    {
        [Route("UploadHandler/Submit")]
        public IHttpActionResult Upload(string id="")
        {
            var files = HttpContext.Current.Request.Files;
            if (files!=null)
            {
                UploadedFile file = new UploadedFile()
                {
                    ID = id
                };
                int len = files.Count;
                string basePath = HttpContext.Current.Server.MapPath("~/Uploads");

                for(int i = 0; i < len; i++)
                {
                    file.FileName = files[i].FileName;
                    file.Size = files[i].ContentLength;
                    file.ContentType = files[i].ContentType;
                    string fullname = Path.Combine(basePath, file.FileName);
                    if (File.Exists(fullname))
                        File.Delete(fullname);
                    try
                    {
                        files[i].SaveAs(fullname);
                    }
                    catch (Exception ex)
                    {
                        file.Error = ex.Message;
                    }
                    if (!string.IsNullOrEmpty(file.Error))
                    {
                        file.PublishedLocation = fullname;
                    }
                }
                return Ok(file);
            }
            else
            {
                return InternalServerError(new Exception("No file(s) uploaded"));
            }

        }
    }
}