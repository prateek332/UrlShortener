using Microsoft.AspNetCore.Http;
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

        [HttpGet("{shortenedUrl}")]
        public IActionResult RedirectToCachedUrl([FromRoute] string shortenedUrl)
        {
            var _ = _memoryCache.TryGetValue(shortenedUrl, out string completeUrl);

            if (completeUrl != null && completeUrl.Length > 0)
            {
                return Redirect(completeUrl);
            }
            return BadRequest("The URL you're making request to doesn't exists or has been removed");
        }
    }
}
