using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Services;

namespace UrlShortner.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        IRandomString _randomString;
        public ApiController(IRandomString randomString)
        {
            _randomString = randomString;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return Ok("API is up and running");
        }

        [HttpGet("/shorten-url")]
        public IActionResult ShortenUrl([FromQuery] string urlToShorten)
        {
            Console.WriteLine(_randomString.GetRandomString(10));
            Console.WriteLine(urlToShorten);
            return Ok();
        }

    }
}
