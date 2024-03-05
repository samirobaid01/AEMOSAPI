using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AEMOS_IdentityA.Models;

namespace AEMOS_IdentityA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly AemosCoreContext _context;

        public AreasController()
        {
            _context = new AemosCoreContext();
        }

        // GET: api/Areas
        [HttpGet]
        public async Task<ActionResult> GetAreas()
        {
            var area= await _context.Areas.Select(area => new { area.Id, area.Name, area.Uuid, area.OrganizationId}).ToListAsync();
            return Ok(area);
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetArea(long id)
        {
            var area = await _context.Areas.FindAsync(id);

            if (area == null)
            {
                return NotFound();
            }

            return Ok(new { area.Id, area.Name, area.Uuid, area.OrganizationId});
        }

        // PUT: api/Areas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArea(long id, Area area)
        {
            if (id != area.Id)
            {
                return BadRequest();
            }

            _context.Entry(area).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreaExists(id))
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

        // POST: api/Areas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Area>> PostArea(Area area)
        {
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArea", new { id = area.Id }, area);
        }

        // DELETE: api/Areas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArea(long id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AreaExists(long id)
        {
            return _context.Areas.Any(e => e.Id == id);
        }
    }
}
