using EndTown.Models.Entities;
using EndTown.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EndTown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService _service;

        public PlatformsController(IPlatformService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var platforms = await _service.GetAllAsync();
            return Ok(platforms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var platform = await _service.GetByIdAsync(id);
            if (platform == null)
                return NotFound(new { message = $"Platform {id} not found" });
            return Ok(platform);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Platform platform)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _service.NameExistsAsync(platform.Name))
                return Conflict(new { message = $"Platform '{platform.Name}' already exists" });

            var created = await _service.CreateAsync(platform);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Platform platform)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _service.NameExistsAsync(platform.Name))
                return Conflict(new { message = $"Platform '{platform.Name}' already exists" });

            var updated = await _service.UpdateAsync(id, platform);
            if (updated == null)
                return NotFound(new { message = $"Platform {id} not found" });

            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Platform {id} not found" });

            return NoContent();
        }
    }
}