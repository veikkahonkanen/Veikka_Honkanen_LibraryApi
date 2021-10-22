using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veikka_Honkanen_LibraryApi.Models;

namespace Veikka_Honkanen_LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteraturesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LiteraturesController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Literatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Literature>>> GetLiteratures()
        {
            return await _context.Literatures.ToListAsync();
        }

        // GET: api/Literatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Literature>> GetLiterature(long id)
        {
            var literature = await _context.Literatures.FindAsync(id);

            if (literature == null)
            {
                return NotFound();
            }

            return literature;
        }

        // PUT: api/Literatures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLiterature(long id, Literature literature)
        {
            if (id != literature.Id)
            {
                return BadRequest();
            }

            _context.Entry(literature).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiteratureExists(id))
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

        // POST: api/Literatures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Literature>> PostLiterature(Literature literature)
        {
            _context.Literatures.Add(literature);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLiterature", new { id = literature.Id }, literature);
        }

        // DELETE: api/Literatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiterature(long id)
        {
            var literature = await _context.Literatures.FindAsync(id);
            if (literature == null)
            {
                return NotFound();
            }

            _context.Literatures.Remove(literature);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LiteratureExists(long id)
        {
            return _context.Literatures.Any(e => e.Id == id);
        }
    }
}
