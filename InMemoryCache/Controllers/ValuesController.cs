using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;

        public ValuesController(IMemoryCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }

        [HttpGet("SetName/{name}")]
        public void SetName(string name)
        {
            memoryCache.Set("name", name);
        }

        [HttpGet("GetName")]
        public string GetName()
        {
            // sadece aşağıdaki satırı yazdığımızda eğer bunu gerçek bir cache uygulamasında yaparsak null değer gelebilir. 
            // aşağıdaki get sonucu null değer gelmesi hata çıkarmaz fakat getten egelecek değer üzerinde işlem uygularsak runtime hataları çıkabilir.
            // bu sebeple tryGetValue metodu ile kontrol sağlayabiliriz.
            // return memoryCache.Get<string>("name"); // <string> alternatifi olarak object olan value yu unboxing ile stringe döndürüp return edebiliriz.

            if (memoryCache.TryGetValue<string>("name", out string name))
            {
                return name.Substring(2);
            }

            return null;
        }

        [HttpGet("SetDate")]
        public void SetDate()
        {
            memoryCache.Set("date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(15)
            });

            // programı çalıştırdığımızda önce setdate metodunu çalıştırıyoruz response ta yazan örn. 21:35:25 süresi yazıyorsa getdate metodunu 21:35:40
            // anına kadar çalıştırmalıyız. çalıştırmadığımızda getdate bize tarih dönmez, veri sıfırlanır (slidingExp). getdate i bu süre içerisinde
            // çalıştırdığımız takdirde de 15 sn içerisinde en az bir kez devamlı çalıştırmalıyız. 15 sn içerisinde tekrar çalıştırılmadığında süre
            // sıfırlanır (slidingExp). 15 sn de bir çalıştırsak da bu verinin ömrü 1 dk dır. 21:36.25 anında veri silinir (absoluteExp).
        }

        [HttpGet("GetDate")]
        public DateTime GetDate()
        {
            return memoryCache.Get<DateTime>("date");
        }
    }
}
