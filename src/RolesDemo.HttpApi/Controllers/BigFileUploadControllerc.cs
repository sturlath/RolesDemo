using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;

namespace RolesDemo.Controllers
{
    [RemoteService]
    [Area("app")]
    [ControllerName("BigFileUpload")]
    [Route("api/app/bigfile")]
    public class BigFileUploadControllerc : RolesDemoController
    {
        [IgnoreAntiforgeryToken]
        [DisableFormValueModelBinding]
        [DisableRequestSizeLimit] //or limit to size [RequestFormLimits(MultipartBodyLengthLimit = 20000L * 1024 * 1024)] //unit is bytes => 2000Mb/2GB
        [HttpPost("upload")]
        public async Task UploadFileAsync()
        {
            if (Request.ContentLength == 0)
                BadRequest();

            try
            {
                var filePath = Path.GetTempFileName();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Request.StreamFile(stream);
                }
            }
            catch (Exception ex)
            {
                //Always end here with
                // "Unexpected end of Stream, the content may have already been read by another component."

                // Asked for help on this here
                // https://github.com/abpframework/abp/issues/10916
                // https://support.abp.io/QA/Questions/2267/Problem-uploading-a-file-using-MultipartReader
                throw;
            }
        }

        [HttpPost("[action]")]
        //[Produces("application/json")]
        //[Consumes("multipart/form-data")]
        public virtual async Task Save(IList<IRemoteStreamContent> UploadRecording) //this parameter needs to be the same as ID in Index.razor! Syncfusion has updated their docs!
        {
            //files are always 0!!

            long size = 0;
            try
            {
                //foreach (var file in UploadFiles)
                //{
                //var filename = ContentDispositionHeaderValue
                //        .Parse(file.ContentDisposition)
                //        .FileName
                //        .Trim('"');

                // filename = Path.Combine(Path.GetTempPath(), $"{filename}");

                ////tempFilename = filePath + $@"\{filename}";
                //size += (int)file.Length;
                //if (!System.IO.File.Exists(filename))
                //{
                //    using (FileStream fs = System.IO.File.Create(filename))
                //    {
                //        file.CopyTo(fs);
                //        fs.Flush();
                //    }
                //}
                //}
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [HttpPost("[action]")]
        public void Remove(IList<IFormFile> UploadFiles)
        {
            try
            {
                var filePath = Path.GetTempFileName();
                var filename = filePath + $@"\{UploadFiles[0].FileName}";
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }
    }
}
