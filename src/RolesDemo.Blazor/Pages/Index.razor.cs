using Blazorise;
using Syncfusion.Blazor.Inputs;
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
        private readonly string saveUrl = "https://localhost:44365/api/app/bigfile/Save";
        private readonly string removeUrl = "https://localhost:44365/api/app/bigfile/Remove";

        public void OnSuccess(SuccessEventArgs args)
        {
            //args.Response.Headers is always missing the CustomHeader but does contain "THIS IS RETURNED!!" in the content-type!
            var customHeader = args.Response.Headers.Split(new char[] { '\n' })[1]; // To split the response header values 

            //customHeader is never returned...there is just one header included..

            if (!string.IsNullOrWhiteSpace(customHeader))
            {
                var key = customHeader.Split(new Char[] { ':' })[0]; // To get the key pair of provided custom data in header 
                var value = customHeader.Split(new Char[] { ':' })[1].Trim(); // To get the value for the key pair of provided custom data in header  
            }
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
