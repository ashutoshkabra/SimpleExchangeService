using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeServiceController : ControllerBase
    {
        // GET: ExchangeServiceController
        public ActionResult Index()
        {
            return Ok(new
            {
                Message = "Exchange Service API",
                StatusCode = 200
            });
        }
    }
}