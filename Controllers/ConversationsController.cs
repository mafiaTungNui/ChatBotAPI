using ChatBotAPI.Data;
using ChatBotAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly ChatBotDbContext _context;

        public ConversationsController(ChatBotDbContext context)
        {
            _context = context;
        }

        // GET: api/Conversations/user/{userId}
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<Conversation>> GetConversationsByUserId(int userId)
        {
            var conversations = _context.Conversations.Where(c => c.UserID == userId).ToList();
            return Ok(conversations);
        }

        // POST: api/Conversations
        [HttpPost]
        public async Task<ActionResult<Conversation>> CreateConversation(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetConversation", new { id = conversation.ConversationID }, conversation);
        }
    }
}
