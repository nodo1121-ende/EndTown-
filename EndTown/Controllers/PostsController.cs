using EndTown.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EndTown.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _service;

        public PostsController(IPostService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _service.GetAllAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _service.GetByIdAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var post = await _service.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
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
        [HttpPost("{id}/like")]
        public async Task<IActionResult> Like(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.LikePostAsync(id, userId);
            if (!result) return BadRequest(new { message = "Already liked or post not found" });
            return Ok(new { message = "Post liked!" });
        }

        [Authorize]
        [HttpDelete("{id}/like")]
        public async Task<IActionResult> Unlike(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.UnlikePostAsync(id, userId);
            if (!result) return NotFound(new { message = "Like not found" });
            return Ok(new { message = "Post unliked!" });
        }
    
    [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments(int id)
        {
            var comments = await _service.GetCommentsAsync(id);
            return Ok(comments);
        }

        [Authorize]
        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] AddCommentRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.AddCommentAsync(id, userId, request.Content);
            if (!result) return NotFound();
            return Ok(new { message = "Comment added!" });
        }
    } 
}
