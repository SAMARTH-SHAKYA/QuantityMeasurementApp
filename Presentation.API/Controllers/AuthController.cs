using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Entity.DTO;
using Presentation.API.Interface;
using System.Threading.Tasks;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IBusinessLogicClient _businessClient;

        public AuthController(IBusinessLogicClient businessClient)
        {
            _businessClient = businessClient;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            try 
            {
                var result = await _businessClient.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Refit.ApiException ex)
            {
                return StatusCode((int)ex.StatusCode, new { Error = ex.Content });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try 
            {
                var result = await _businessClient.LoginAsync(dto);
                return Ok(result);
            }
            catch (Refit.ApiException ex)
            {
                return StatusCode((int)ex.StatusCode, new { Error = ex.Content });
            }
        }
    }
}
