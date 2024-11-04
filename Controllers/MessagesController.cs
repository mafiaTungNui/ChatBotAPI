using ChatBotAPI.Data;
using ChatBotAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ChatBotDbContext _context;

        public MessagesController(ChatBotDbContext context)
        {
            _context = context;
        }

        // GET: api/Messages?conversationId={conversationId}
        [HttpGet]
        public ActionResult<IEnumerable<Message>> GetMessages(int conversationId)
        {
            var messages = _context.Messages.Where(m => m.ConversationID == conversationId).ToList();
            return Ok(messages);
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<Message>> SendMessage(Message message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.MessageText))
            {
                return BadRequest("Tin nhắn không hợp lệ.");
            }

            try
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                // Lấy intent từ tin nhắn và tìm câu trả lời phù hợp
                var intent = await _context.Intents.FirstOrDefaultAsync(i => i.IntentName == message.MessageText);
                if (intent != null)
                {
                    var response = await _context.Responses.FirstOrDefaultAsync(r => r.IntentID == intent.IntentID);
                    if (response != null)
                    {
                        var botMessage = new Message
                        {
                            ConversationID = message.ConversationID,
                            Sender = "Bot",
                            MessageText = response.ResponseText,
                            Timestamp = System.DateTime.Now
                        };

                        _context.Messages.Add(botMessage);
                        await _context.SaveChangesAsync();
                        return Ok(botMessage); // Trả về tin nhắn từ bot
                    }
                }

                return Ok(message); // Trả về tin nhắn gốc nếu không có phản hồi
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}"); // Trả về thông báo lỗi
            }
        }

    }
}
