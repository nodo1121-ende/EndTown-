using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EndTown.Data;
using EndTown.Models.Entities;

namespace EndTown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly EndTownDbContext _context;

        public PlatformsController(EndTownDbContext context)
        {
            _context = context;
        }

        // GET: api/platforms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms()
        {
            var platforms = await _context.Platforms.ToListAsync();
            return Ok(platforms);
        }

        // GET: api/platforms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Platform>> GetPlatform(int id)
        {
            var platform = await _context.Platforms.FindAsync(id);

            if (platform == null)
            {
                return NotFound(new { message = $"Platform with ID {id} not found" });
            }

            return Ok(platform);
        }

        // POST: api/platforms
        [Authorize] // მხოლოდ ავთენტიფიცირებული მომხმარებლებისთვის
        [HttpPost]
        public async Task<ActionResult<Platform>> CreatePlatform(Platform platform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // შევამოწმოთ უნიკალურობა
            var existingPlatform = await _context.Platforms
                .FirstOrDefaultAsync(p => p.Name.ToLower() == platform.Name.ToLower());

            if (existingPlatform != null)
            {
                return Conflict(new { message = $"Platform with name '{platform.Name}' already exists" });
            }

            platform.CreatedAt = DateTime.UtcNow;
            platform.UpdatedAt = DateTime.UtcNow;

            _context.Platforms.Add(platform);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlatform), new { id = platform.Id }, platform);
        }

        // PUT: api/platforms/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlatform(int id, Platform platform)
        {
            if (id != platform.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPlatform = await _context.Platforms.FindAsync(id);
            if (existingPlatform == null)
            {
                return NotFound();
            }

            // შევამოწმოთ უნიკალურობა (თუ სახელი შეცვლილია)
            if (existingPlatform.Name != platform.Name)
            {
                var duplicatePlatform = await _context.Platforms
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == platform.Name.ToLower());

                if (duplicatePlatform != null)
                {
                    return Conflict(new { message = $"Platform with name '{platform.Name}' already exists" });
                }
            }

            // განახლება
            existingPlatform.Name = platform.Name;
            existingPlatform.Description = platform.Description;
            existingPlatform.LogoUrl = platform.LogoUrl;
            existingPlatform.BannerUrl = platform.BannerUrl;
            existingPlatform.RegistrationOpen = platform.RegistrationOpen;
            existingPlatform.PublicAccess = platform.PublicAccess;
            existingPlatform.MaxPostLength = platform.MaxPostLength;
            existingPlatform.MaxCommentLength = platform.MaxCommentLength;
            existingPlatform.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlatformExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // PATCH: api/platforms/5/statistics
        [Authorize]
        [HttpPatch("{id}/statistics")]
        public async Task<IActionResult> UpdateStatistics(int id, [FromBody] StatisticsUpdateDto statistics)
        {
            var platform = await _context.Platforms.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            platform.UpdateStatistics(
                statistics.TotalUsers,
                statistics.TotalPosts,
                statistics.TotalComments,
                statistics.TotalLikes
            );

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/platforms/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatform(int id)
        {
            var platform = await _context.Platforms.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            _context.Platforms.Remove(platform);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/platforms/check-name?name=test
        [HttpGet("check-name")]
        public async Task<ActionResult<bool>> CheckPlatformName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { message = "Name is required" });
            }

            var exists = await _context.Platforms
                .AnyAsync(p => p.Name.ToLower() == name.ToLower());

            return Ok(new { exists = exists });
        }

        // GET: api/platforms/registration-status/5
        [HttpGet("registration-status/{id}")]
        public async Task<ActionResult<bool>> GetRegistrationStatus(int id)
        {
            var platform = await _context.Platforms.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            return Ok(new { canRegister = platform.CanUserRegister() });
        }

        private bool PlatformExists(int id)
        {
            return _context.Platforms.Any(e => e.Id == id);
        }
    }

    // DTO სტატისტიკის განახლებისთვის
    public class StatisticsUpdateDto
    {
        public int TotalUsers { get; set; }
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
    }
}
