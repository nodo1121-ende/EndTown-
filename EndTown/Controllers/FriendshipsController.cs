using EndTown.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EndTown.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendshipService _service;

        public FriendshipsController(IFriendshipService service)
        {
            _service = service;
        }

        [HttpPost("send/{receiverId}")]
        public async Task<IActionResult> SendRequest(int receiverId)
        {
            var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.SendRequestAsync(senderId, receiverId);
            if (!result) return BadRequest(new { message = "Request already exists" });
            return Ok(new { message = "Friend request sent!" });
        }

        [HttpPut("accept/{friendshipId}")]
        public async Task<IActionResult> AcceptRequest(int friendshipId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.AcceptRequestAsync(friendshipId, userId);
            if (!result) return NotFound();
            return Ok(new { message = "Friend request accepted!" });
        }

        [HttpPut("reject/{friendshipId}")]
        public async Task<IActionResult> RejectRequest(int friendshipId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _service.RejectRequestAsync(friendshipId, userId);
            if (!result) return NotFound();
            return Ok(new { message = "Friend request rejected!" });
        }

        [HttpGet("friends")]
        public async Task<IActionResult> GetFriends()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var friends = await _service.GetFriendsAsync(userId);
            return Ok(friends);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var requests = await _service.GetPendingRequestsAsync(userId);
            return Ok(requests);
        }
    }
}