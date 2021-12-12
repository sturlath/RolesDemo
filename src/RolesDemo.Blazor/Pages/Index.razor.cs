using Blazorise;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RolesDemo.Blazor.Pages
{
    public partial class Index
    {
        public Index()
        {
        }

        private async Task OnFileUploadChanged(FileChangedEventArgs e)
        {
            if (e.Files != null && e.Files.Length == 1)
            {
                var file = e.Files[0];

                using (var ms = file.OpenReadStream(file.Size))
                {
                    var content = new MultipartFormDataContent();
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    content.Add(new StreamContent(ms, Convert.ToInt32(ms.Length)), file.Type, file.Name);
                    // add additional data https://brokul.dev/sending-files-and-additional-data-using-httpclient-in-net-core

                    using var client = new HttpClient();
                    //post the file like this since I had problems with passing MultipartFormDataContent to AppService
                    var postResult = await client.PostAsync("https://localhost:44365/api/app/bigfile/upload", content);
                }
            }
        }
    }

}
