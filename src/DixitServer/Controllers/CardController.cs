using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DixitServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var file = @"d:\WS\Dixit\src\PlayerCards\Assets\dixit_0009.jpg";
            using var stream = new MemoryStream(System.IO.File.ReadAllBytes(file).ToArray());
            //var formFile = new FormFile(stream, 0, stream.Length, "streamFile", file.Split(@"\").Last());

            var formFile = new FormFile(stream, 0, stream.Length, null, file.Split(@"\").Last())
            {
                Headers = new HeaderDictionary(),
                ContentType = "multipart/form-data"
            };

            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
            byte[] fileData = System.IO.File.ReadAllBytes(file);

            //if (fileData == null)
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
            //S3:Set Response contents and MediaTypeHeaderValue
            Response.Content = new ByteArrayContent(fileData);
            //Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            Response.Content.Headers.ContentLength = fileData.Length;
            Response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            //var stream = new MemoryStream();
            //Image img = Image.FromFile(@"d:\WS\Dixit\src\PlayerCards\Assets\dixit_0009.jpg");
            //img.Save(stream, ImageFormat.Jpeg);
            //// processing the stream.

            //var result = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new ByteArrayContent(stream.ToArray())
            //};
            //result.Content.Headers.ContentDisposition =
            //    new ContentDispositionHeaderValue("attachment")
            //    {
            //        FileName = @"dixit_0009.jpg"
            //    };
            //result.Content.Headers.ContentType =
            //    new MediaTypeHeaderValue("image/jpeg");

            //using var stream = new MemoryStream(File.ReadAllBytes(file).ToArray());
            //var formFile = new FormFile(stream, 0, stream.Length, "streamFile", file.Split(@"\").Last());

            //return result;
            //Byte[] b = System.IO.File.ReadAllBytes(@"E:\\Test.jpg");   // You can use your own method over here.         
            return File(fileData, "image/jpeg");

            //return Response;
        }

    }
}
