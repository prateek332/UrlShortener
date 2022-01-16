using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortner.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Index()
        {
            return Ok("API is up and running");
        }
    }
}
