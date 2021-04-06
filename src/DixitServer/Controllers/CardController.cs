using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

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

            var formFile = new FormFile(stream, 0, stream.Length, null, file.Split(@"\").Last())
            {
                Headers = new HeaderDictionary(),
                ContentType = "multipart/form-data"
            };

            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
            byte[] fileData = System.IO.File.ReadAllBytes(file);

            Response.Content = new ByteArrayContent(fileData);
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            Response.Content.Headers.ContentLength = fileData.Length;
            Response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            
            return File(fileData, "image/jpeg");
        }

    }
}
