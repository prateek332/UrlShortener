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
        const int urlCacheTime = 1; // in days

        readonly IRandomString _randomString;
        readonly IMemoryCache _memoryCache;
        readonly IConfiguration _configuration;
        public ApiController(IMemoryCache memoryCache, IRandomString randomString, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _randomString = randomString;
            _configuration = configuration;
        }

        // GET -> /api
        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Index()
        {
            return Ok($"API is up and running. To learn more about api visit {_configuration["ApiDocsUrl"]}");
        }

        // POST -> /api/shorten-url
        [HttpPost("shorten-url")]
        [ProducesResponseType(typeof(ShortenUrl), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ShortenUrl([FromBody] ShortenUrl url)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request body contains invalid parameters");
            }
            
            // get the cached entry or create a new entry if cache doesn't exists
            var cacheEntry = await _memoryCache.GetOrCreateAsync(url.UrlToShorten, entry =>
            {
                // generate a random string for appending at the end of shortened url
                string randomString = _randomString.GetRandomString(randomUrlStringLength);

                // cached entry would be removed after 24hrs
                entry.SlidingExpiration = TimeSpan.FromDays(urlCacheTime);

                return Task.FromResult(randomString);
            });

            // if no cached entry was found, then create another cache with
            // (key, value) = (cache, urlToShorten), so when user request to shortened url
            // the user is redirected to cached url
            _memoryCache.Set(cacheEntry, url.UrlToShorten);

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

    // utility class for /api/shorten-url POST method request body
    public class ShortenUrl
    {
        public string? UrlToShorten { get; set; }
    }
}
