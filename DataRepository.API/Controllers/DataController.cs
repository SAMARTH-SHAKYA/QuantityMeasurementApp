using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Entity;
using QuantityMeasurementApp.Entity.Entities;
using QuantityMeasurementApp.Repository.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataRepository.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DataController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("users/{username}")]
        public async Task<ActionResult<UserEntity>> GetUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("users")]
        public async Task<ActionResult> CreateUser([FromBody] UserEntity user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("measurements")]
        public async Task<ActionResult<List<QuantityMeasurementEntity>>> GetAllMeasurements()
        {
            return await _context.Measurements.ToListAsync();
        }

        [HttpPost("measurements")]
        public async Task<ActionResult> SaveMeasurement([FromBody] QuantityMeasurementEntity measurement)
        {
            _context.Measurements.Add(measurement);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
