using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace UrlShortner.Controllers
{
    [Route("/")]
    [ApiController]
    public class Index : ControllerBase
    {

        readonly IMemoryCache _memoryCache;

        public Index(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        // GET -> /some_random_string
        [HttpGet("{shortenedUrl}")]
        public IActionResult RedirectToCachedUrl([FromRoute] string shortenedUrl)
        {
            var _ = _memoryCache.TryGetValue(shortenedUrl, out string completeUrl);

            // If URL is cached then try to form a complete url and redirect user to it
            // If URL couldn't be generated, then a 400 error is generated with the exception message
            if (completeUrl != null && completeUrl.Length > 0)
            {
                UriBuilder builder;
                try
                {
                   builder = new UriBuilder(completeUrl);
                } 
                catch (UriFormatException ex)
                {
                    return BadRequest(ex.Message);
                }
                return Redirect(builder.ToString());
            }
            return BadRequest("The URL you're making request to doesn't exists or has been removed");
        }
    }
}
