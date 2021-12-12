using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;

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
        [DisableRequestSizeLimit]
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
                throw;
            }
        }
    }
}
