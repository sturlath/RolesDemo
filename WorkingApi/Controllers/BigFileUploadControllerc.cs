using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WorkingApi.Controllers
{
    [Area("app")]
    [ApiController]
    [Route("api/app/bigfile")]
    public class BigFileUploadControllerc : ControllerBase
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
                //No exception!
            }
        }
    }

}
