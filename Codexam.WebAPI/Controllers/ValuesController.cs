using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codexam.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // Bu, GET isteği için örnek bir metod.
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            // Örnek veri döndürüyoruz.
            return new string[] { "value1", "value2" };
        }

        // Bu, POST isteği için örnek bir metod.
        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            // Gelen veriyi aldıktan sonra basit bir işlem yapıyoruz.
            return Ok($"Received value: {value}");
        }
    }
}
