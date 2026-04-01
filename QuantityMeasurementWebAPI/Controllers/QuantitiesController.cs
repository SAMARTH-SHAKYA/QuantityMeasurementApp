using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Entity;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Security.Claims;

namespace QuantityMeasurementWebAPI.Controllers
{
    [ApiController]
    [Route("api/measurement")]
    [Authorize]
    public class QuantitiesController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;
        private readonly IQuantityMeasurementRepository _repository;
        private readonly IDistributedCache _cache;
        private const string HistoryCacheKey = "MeasurementHistory";

        public QuantitiesController(IQuantityMeasurementService service, IQuantityMeasurementRepository repository, IDistributedCache cache)
        {
            _service = service;
            _repository = repository;
            _cache = cache;
        }

        private int? GetCurrentUserId()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                // "uid" is our custom claim - unambiguous, never remapped by .NET JWT middleware
                var idClaim = User.FindFirst("uid");
                if (idClaim != null && int.TryParse(idClaim.Value, out int id))
                {
                    return id;
                }
            }
            return null;
        }

        private void SafeRemoveCache(string key)
        {
            try { _cache.Remove(key); }
            catch (Exception ex) { Console.WriteLine($"Cache failure: {ex.Message}"); }
        }

        public class ConvertRequest
        {
            public QuantityDTO Source { get; set; } = null!;
            public string TargetUnit { get; set; } = string.Empty;
        }

        public class CompareRequest
        {
            public QuantityDTO Quantity1 { get; set; } = null!;
            public QuantityDTO Quantity2 { get; set; } = null!;
        }

        public class ArithmeticRequest
        {
            public QuantityDTO Quantity1 { get; set; } = null!;
            public QuantityDTO Quantity2 { get; set; } = null!;
            public string TargetUnit { get; set; } = string.Empty;
        }

        [HttpPost("convert")]
        [AllowAnonymous]
        public IActionResult Convert([FromBody] ConvertRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = _service.Convert(request.Source, request.TargetUnit, userId);
                if (userId.HasValue) SafeRemoveCache($"{HistoryCacheKey}_{userId.Value}");
                return Ok(result);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("compare")]
        [AllowAnonymous]
        public IActionResult Compare([FromBody] CompareRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = _service.Compare(request.Quantity1, request.Quantity2, userId);
                bool areEqual = result.Value == 1.0;
                if (userId.HasValue) SafeRemoveCache($"{HistoryCacheKey}_{userId.Value}");
                return Ok(new { AreEqual = areEqual });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("add")]
        [AllowAnonymous]
        public IActionResult Add([FromBody] ArithmeticRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = _service.Add(request.Quantity1, request.Quantity2, request.TargetUnit, userId);
                if (userId.HasValue) SafeRemoveCache($"{HistoryCacheKey}_{userId.Value}");
                return Ok(result);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("subtract")]
        [AllowAnonymous]
        public IActionResult Subtract([FromBody] ArithmeticRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = _service.Subtract(request.Quantity1, request.Quantity2, request.TargetUnit, userId);
                if (userId.HasValue) SafeRemoveCache($"{HistoryCacheKey}_{userId.Value}");
                return Ok(result);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("multiply")]
        [AllowAnonymous]
        public IActionResult Multiply([FromBody] ArithmeticRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = _service.Multiply(request.Quantity1, request.Quantity2, request.TargetUnit, userId);
                if (userId.HasValue) SafeRemoveCache($"{HistoryCacheKey}_{userId.Value}");
                return Ok(result);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("divide")]
        [AllowAnonymous]
        public IActionResult Divide([FromBody] ArithmeticRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                // The backend Divide interface typically takes two args, returning Ratio
                var result = _service.Divide(request.Quantity1, request.Quantity2, userId);
                if (userId.HasValue) SafeRemoveCache($"{HistoryCacheKey}_{userId.Value}");
                return Ok(result);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("history")]
        public ActionResult<List<QuantityMeasurementEntity>> GetHistory()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue) return Unauthorized("User is not logged in.");
                
                string userCacheKey = $"{HistoryCacheKey}_{userId.Value}";

                try
                {
                    var cachedHistory = _cache.GetString(userCacheKey);
                    if (!string.IsNullOrEmpty(cachedHistory))
                    {
                        var cachedResults = JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(cachedHistory);
                        return Ok(cachedResults);
                    }
                }
                catch (Exception ex) { Console.WriteLine($"Cache read failure: {ex.Message}"); }

                var results = _repository.GetMeasurementsForUser(userId.Value);

                try
                {
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };
                    _cache.SetString(userCacheKey, JsonSerializer.Serialize(results), cacheOptions);
                }
                catch (Exception ex) { Console.WriteLine($"Cache write failure: {ex.Message}"); }

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unable to retrieve history: {ex.Message}");
            }
        }
    }
}
