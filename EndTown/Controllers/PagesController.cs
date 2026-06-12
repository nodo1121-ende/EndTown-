using EndTown.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EndTown.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagesController : ControllerBase
    {
        private readonly IPageService _service;

        public PagesController(IPageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var page = await _service.GetByIdAsync(id);
            if (page == null) return NotFound();
            return Ok(page);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePageRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var page = await _service.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = page.Id }, page);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var deleted = await _service.DeleteAsync(id, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [Authorize]
        [HttpPost("{id}/follow")]
        public async Task<IActionResult> Follow(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.FollowAsync(id, userId);
            if (!result) return BadRequest(new { message = "Already following" });
            return Ok(new { message = "Page followed!" });
        }

        [Authorize]
        [HttpDelete("{id}/follow")]
        public async Task<IActionResult> Unfollow(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.UnfollowAsync(id, userId);
            if (!result) return NotFound();
            return Ok(new { message = "Page unfollowed!" });
        }
    }
}