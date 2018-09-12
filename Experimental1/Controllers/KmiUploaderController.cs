using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Experimental1.Models;

namespace Experimental1.Controllers
{
    [RoutePrefix("api")]
    public class KmiUploaderController : ApiController
    {
        [Route("KmiUploader/Upload/{uid}")]
        public async Task<IHttpActionResult> Upload(string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return InternalServerError(new Exception("'uid' cannot be null or empty"));
            var files = HttpContext.Current.Request.Files;
            if (files != null)
            {
                var server = HttpContext.Current.Server;
                string basePath = server.MapPath($"~/Uploads/{uid}");
                if (!Directory.Exists(basePath))
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

                for (int i = 0; i < len; i++)
                {
                    file.FileName = files[i].FileName;
                    file.Size = files[i].ContentLength;
                    file.ContentType = files[i].ContentType;
                    string virtualFullname = $"~/Uploads/{uid}/" + file.FileName;
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
        [Route("KmiUploader/GetData/{uid}")]
        public IHttpActionResult GetData(string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return InternalServerError(new Exception("'uid' cannot be null or empty"));
            string basePath = $"~/Uploads/{uid}";
            HttpServerUtility server = HttpContext.Current.Server;
            List<DownloadedFile> files = new List<DownloadedFile>();
            if (!Directory.Exists(server.MapPath(basePath)))
                return Ok(files);
            foreach(FileInfo file in new DirectoryInfo(server.MapPath(basePath)).GetFiles().Where(x=>!string.IsNullOrEmpty(x.Extension)))
            {
                DownloadedFile downloadedFile = new DownloadedFile
                {
                    FileName = file.Name,
                    PublishedLocation = $"/Uploads/{uid}/{file.Name}",
                    Size = file.Length,
                    Extension = file.Extension
                };
                files.Add(downloadedFile);
            }
            return Ok(files);
        }
    }
}
