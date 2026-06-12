using EndTown.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EndTown.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _service;

        public GroupsController(IGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var group = await _service.GetByIdAsync(id);
            if (group == null) return NotFound();
            return Ok(group);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var group = await _service.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = group.Id }, group);
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
        [HttpPost("{id}/join")]
        public async Task<IActionResult> Join(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.JoinAsync(id, userId);
            if (!result) return BadRequest(new { message = "Already a member" });
            return Ok(new { message = "Joined group!" });
        }

        [Authorize]
        [HttpDelete("{id}/leave")]
        public async Task<IActionResult> Leave(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.LeaveAsync(id, userId);
            if (!result) return NotFound();
            return Ok(new { message = "Left group!" });
        }
    }
}