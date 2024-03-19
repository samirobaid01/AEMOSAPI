using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AEMOSAPI.Models;
using AEMOSAPI.DTO;

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
            List<AreaDTO> _area = await (from ar in _context.Areas
                                    join org in _context.Organizations on ar.OrganizationId equals org.Id
                                    select new AreaDTO() { Id = ar.Id,Name=ar.Name, OrgName = org.Name, description = ar.Description }).ToListAsync();
            return Ok(_area);
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetArea(long id)
        {
            AreaDTO? _area = await (from ar in _context.Areas
                         join org in _context.Organizations on ar.OrganizationId equals org.Id where ar.Id == id
                         select new AreaDTO() { Id=ar.Id, OrgName=org.Name, description=ar.Description }).FirstOrDefaultAsync();
                        

            if (_area == null)
            {
                return NotFound();
            }

            return Ok(_area);
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
