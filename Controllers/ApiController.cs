using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Services;
using Microsoft.Extensions.Caching.Memory;

namespace UrlShortner.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        const int randomUrlStringLength = 7;
        readonly IRandomString _randomString;
        readonly IMemoryCache _memoryCache;
        public ApiController(IMemoryCache memoryCache, IRandomString randomString)
        {
            _memoryCache = memoryCache;
            _randomString = randomString;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return Ok("API is up and running");
        }

        [HttpGet("shorten-url/{urlToShorten}")]
        public async Task<IActionResult> ShortenUrl([FromRoute] string urlToShorten)
        {
            // get the cached entry or create a new entry if cache doesn't exists
            var cacheEntry = await _memoryCache.GetOrCreateAsync(urlToShorten, entry =>
            {
                // generate a random string for appending at the end of shortened url
                string randomString = _randomString.GetRandomString(randomUrlStringLength);

                // cached entry would be removed after 24hrs
                entry.SlidingExpiration = TimeSpan.FromDays(1);

                return Task.FromResult(randomString);
            });

            // if no cached entry was found, then create another cache with
            // (key, value) = (cache, urlToShorten), so when user request to shortened url
            // the user is redirected to cached url
            if (cacheEntry.Length == 0)
            {
                _memoryCache.Set(cacheEntry, urlToShorten);
            }

            string shortenUrl = generateShortenUrl(cacheEntry);
            return Ok(new
            {
                shortenUrl = shortenUrl,
            });
        }

        private string generateShortenUrl(string randomString)
        {
            return $"https://{Request.Host.ToUriComponent()}/{randomString}";
        }
    }
}
