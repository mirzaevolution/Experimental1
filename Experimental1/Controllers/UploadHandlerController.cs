//https://stackoverflow.com/questions/40632028/asp-net-how-read-a-multipart-form-data-in-web-api

using Experimental1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace Experimental1.Controllers
{
    public class UploadHandlerController : ApiController
    {
        [Route("UploadHandler/Submit")]
        public async Task<IHttpActionResult> Upload()
        {
           
            var files = HttpContext.Current.Request.Files;
            if (files!=null)
            {
                var server = HttpContext.Current.Server;
                string basePath = server.MapPath("~/Uploads");
                if (Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(basePath);
                await Request.Content.ReadAsMultipartAsync(provider);

                UploadedFile file = new UploadedFile()
                {
                    ID = provider.FormData["id"] == null ? Guid.NewGuid().ToString("n") : provider.FormData["id"].ToString()
                };
                int len = files.Count;
            
                for(int i = 0; i < len; i++)
                {
                    file.FileName = files[i].FileName;
                    file.Size = files[i].ContentLength;
                    file.ContentType = files[i].ContentType;
                    string virtualFullname = "~/Uploads/" + file.FileName;
                    string fullname = server.MapPath(virtualFullname);
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
                    if (string.IsNullOrEmpty(file.Error))
                    {

                        file.PublishedLocation = virtualFullname.Replace("~", "");
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