using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace DistributedCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDistributedCache distributedCache;

        public ValuesController(IDistributedCache _distributedCache)
        {
            distributedCache = _distributedCache;
        }

        [HttpGet("Set")]
        public async Task<IActionResult> Set(string name, string surname)
        {
            await distributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(15)
            });
            await distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(15)
            });
            return Ok();
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var name = await distributedCache.GetStringAsync("name");
            var surnameBinary = await distributedCache.GetAsync("surname");
            var surname = Encoding.UTF8.GetString(surnameBinary);
            return Ok(new
            {
                name,
                surname
            });
        }
    }
}
