using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatBotAPI.Data;
using ChatBotAPI.Models;

namespace ChatBotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntentsController : ControllerBase
    {
        private readonly ChatBotDbContext _context;

        public IntentsController(ChatBotDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Intent>>> GetIntents()
        {
            return await _context.Intents.Include(i => i.Responses).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Intent>> GetIntent(int id)
        {
            var intent = await _context.Intents.Include(i => i.Responses).FirstOrDefaultAsync(i => i.IntentID == id);

            if (intent == null)
            {
                return NotFound();
            }

            return intent;
        }

        [HttpPost]
        public async Task<ActionResult<Intent>> PostIntent(Intent intent)
        {
            _context.Intents.Add(intent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIntent", new { id = intent.IntentID }, intent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntent(int id, Intent intent)
        {
            if (id != intent.IntentID)
            {
                return BadRequest();
            }

            _context.Entry(intent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntent(int id)
        {
            var intent = await _context.Intents.FindAsync(id);
            if (intent == null)
            {
                return NotFound();
            }

            _context.Intents.Remove(intent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IntentExists(int id)
        {
            return _context.Intents.Any(e => e.IntentID == id);
        }
    }
}
