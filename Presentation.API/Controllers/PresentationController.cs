using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Entity;
using Presentation.API.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/measurement")]
    public class PresentationController : ControllerBase
    {
        private readonly IBusinessLogicClient _businessClient;

        public PresentationController(IBusinessLogicClient businessClient)
        {
            _businessClient = businessClient;
        }

        [HttpPost("convert")]
        public async Task<IActionResult> Convert([FromBody] object request)
        {
            try { return Ok(await _businessClient.ConvertAsync(request)); }
            catch (Refit.ApiException ex) { return StatusCode((int)ex.StatusCode, new { Error = ex.Content }); }
        }

        [HttpPost("compare")]
        public async Task<IActionResult> Compare([FromBody] object request)
        {
            try { return Ok(await _businessClient.CompareAsync(request)); }
            catch (Refit.ApiException ex) { return StatusCode((int)ex.StatusCode, new { Error = ex.Content }); }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] object request)
        {
            try { return Ok(await _businessClient.AddAsync(request)); }
            catch (Refit.ApiException ex) { return StatusCode((int)ex.StatusCode, new { Error = ex.Content }); }
        }

        [HttpPost("subtract")]
        public async Task<IActionResult> Subtract([FromBody] object request)
        {
            try { return Ok(await _businessClient.SubtractAsync(request)); }
            catch (Refit.ApiException ex) { return StatusCode((int)ex.StatusCode, new { Error = ex.Content }); }
        }

        [HttpPost("multiply")]
        public async Task<IActionResult> Multiply([FromBody] object request)
        {
            try { return Ok(await _businessClient.MultiplyAsync(request)); }
            catch (Refit.ApiException ex) { return StatusCode((int)ex.StatusCode, new { Error = ex.Content }); }
        }

        [HttpPost("divide")]
        public async Task<IActionResult> Divide([FromBody] object request)
        {
            try { return Ok(await _businessClient.DivideAsync(request)); }
            catch (Refit.ApiException ex) { return StatusCode((int)ex.StatusCode, new { Error = ex.Content }); }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            try { return Ok(await _businessClient.GetHistoryAsync()); }
            catch (Refit.ApiException ex) { return StatusCode((int)ex.StatusCode, new { Error = ex.Content }); }
        }
    }
}
