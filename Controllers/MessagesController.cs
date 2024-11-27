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
        private string? FindClosestIntent(string userInput)
        {
            var intents = _context.Intents.Select(i => i.IntentName).ToList();
            string? closestIntent = null;
            int minDistance = int.MaxValue;

            foreach (var intent in intents)
            {
                int distance = CalculateLevenshteinDistance(userInput, intent);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIntent = intent;
                }
            }

            // Trả về intent nếu khoảng cách nhỏ hơn một ngưỡng (ví dụ: 3)
            return minDistance <= 3 ? closestIntent : null;
        }

        private int CalculateLevenshteinDistance(string source, string target)
        {
            var sourceLength = source.Length;
            var targetLength = target.Length;
            var matrix = new int[sourceLength + 1, targetLength + 1];

            for (var i = 0; i <= sourceLength; i++) matrix[i, 0] = i;
            for (var j = 0; j <= targetLength; j++) matrix[0, j] = j;

            for (var i = 1; i <= sourceLength; i++)
            {
                for (var j = 1; j <= targetLength; j++)
                {
                    var cost = source[i - 1] == target[j - 1] ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost
                    );
                }
            }

            return matrix[sourceLength, targetLength];
        }

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
        [HttpPost]
        public async Task<ActionResult<Message>> SendMessage(Message message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.MessageText))
            {
                return BadRequest("Tin nhắn không hợp lệ.");
            }

            try
            {
                // Thêm tin nhắn của người dùng vào cơ sở dữ liệu
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                // Tìm intent khớp chính xác
                var intent = await _context.Intents.FirstOrDefaultAsync(i => i.IntentName == message.MessageText);
                if (intent != null)
                {
                    // Nếu tìm thấy intent, trả về phản hồi
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
                        return Ok(botMessage);
                    }
                }

                // Nếu không tìm thấy intent chính xác, tìm intent gần đúng
                var closestIntent = FindClosestIntent(message.MessageText);
                if (!string.IsNullOrEmpty(closestIntent))
                {
                    var botMessage = new Message
                    {
                        ConversationID = message.ConversationID,
                        Sender = "Bot",
                        MessageText = $"Bạn có phải muốn nói: '{closestIntent}' không?",
                        Timestamp = System.DateTime.Now
                    };

                    _context.Messages.Add(botMessage);
                    await _context.SaveChangesAsync();
                    return Ok(botMessage);
                }

                // Nếu không tìm thấy intent gần đúng, trả về phản hồi mặc định
                var fallbackMessage = new Message
                {
                    ConversationID = message.ConversationID,
                    Sender = "Bot",
                    MessageText = "Xin lỗi, tôi không hiểu ý của bạn. Bạn có thể thử diễn đạt lại không?",
                    Timestamp = System.DateTime.Now
                };

                _context.Messages.Add(fallbackMessage);
                await _context.SaveChangesAsync();
                return Ok(fallbackMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }
    }

}
