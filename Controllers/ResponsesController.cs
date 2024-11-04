using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatBotAPI.Data;
using ChatBotAPI.Models;

namespace ChatBotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsesController : ControllerBase
    {
        private readonly ChatBotDbContext _context;

        public ResponsesController(ChatBotDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Response>>> GetResponses()
        {
            return await _context.Responses.Include(r => r.IntentNavigation).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetResponse(int id)
        {
            var response = await _context.Responses.Include(r => r.IntentNavigation).FirstOrDefaultAsync(r => r.ResponseID == id);

            if (response == null)
            {
                return NotFound();
            }

            return response;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> PostResponse(Response response)
        {
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResponse", new { id = response.ResponseID }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutResponse(int id, Response response)
        {
            if (id != response.ResponseID)
            {
                return BadRequest();
            }

            _context.Entry(response).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponseExists(id))
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
        public async Task<IActionResult> DeleteResponse(int id)
        {
            var response = await _context.Responses.FindAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            _context.Responses.Remove(response);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResponseExists(int id)
        {
            return _context.Responses.Any(e => e.ResponseID == id);
        }
    }
}
