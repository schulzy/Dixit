using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        HashSet<string> _listOfPath = new HashSet<string>();

        [HttpGet]
        public IActionResult Get()
        {
            string file = GetRandomFilePath();
            using var stream = new MemoryStream(System.IO.File.ReadAllBytes(file).ToArray());

            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
            byte[] fileData = System.IO.File.ReadAllBytes(file);
            var bla = GetHashSHA1(fileData);

            Response.Content = new ByteArrayContent(fileData);
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            Response.Content.Headers.ContentLength = fileData.Length;
            Response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            
            return File(fileData, "image/jpeg", file.Split(@"\").Last());
        }

        private string GetHashSHA1(byte[] data)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                return string.Concat(sha1.ComputeHash(data).Select(x => x.ToString("X2")));
            }
        }

        private string GetRandomFilePath()
        {
            var paths = Directory.GetFiles(Path.Combine(AssemblyDirectory, "Cards"));

            int num = paths.Length;
            string path = paths[_rnd.Next(0, num - 1)];
            while(!_listOfPath.Add(path))
                path = paths[_rnd.Next(0, num - 1)];

            return path;
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
