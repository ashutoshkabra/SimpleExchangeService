#region Using Namespaces

using Microsoft.AspNetCore.Mvc;

using ExchangeServiceAPI.Dtos;
using ExchangeServiceAPI.Services;

#endregion

namespace ExchangeServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeServiceController : ControllerBase
    {
        #region Internal Members

        private IForexServices _forexServices;

        #endregion

        #region Constructors

        public ExchangeServiceController(IForexServices forexServices)
        {
            _forexServices = forexServices;
        }

        #endregion

        // GET: ExchangeServiceController
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(new
            {
                Message = "Exchange Service API",
                StatusCode = 200
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExchangeRequest request)
        {
            string msg = _forexServices.ValidateExchangeRequest(request);

            // Check for input validation
            if (!string.IsNullOrEmpty(msg))
            {
                return BadRequest(new
                {
                    message = msg,
                    statusCode = 400
                });
            }

            // Input Validation Ok
            decimal? exchangeValue = await _forexServices.CalculateExchangeRate(request.InputCurrency, request.OutputCurrency, request.Amount);

            if (exchangeValue != null)
            {
                ExchangeResponse response = new ExchangeResponse
                {
                    Amount = request.Amount,
                    InputCurrency = request.InputCurrency,
                    OutputCurrency = request.OutputCurrency,
                    Value = exchangeValue.Value
                };

                return Ok(response);
            }
            else
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}