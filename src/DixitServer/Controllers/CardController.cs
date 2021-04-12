using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace DixitServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        Random _rnd = new Random();

        [HttpGet]
        public IActionResult Get()
        {
            string file = GetRandomFilePath();
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

        private string GetRandomFilePath()
        {
            var paths = Directory.GetFiles(Path.Combine(AssemblyDirectory, "Cards"));

            int num = paths.Length;

            return paths[_rnd.Next(0, num - 1)];
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
