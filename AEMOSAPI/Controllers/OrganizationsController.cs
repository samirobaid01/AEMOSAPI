using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AEMOSAPI.DTO;
using AEMOSAPI.Models;

namespace AEMOS_IdentityA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly AemosCoreContext _context;

        public OrganizationsController()
        {
            _context = new AemosCoreContext();
        }

        // GET: api/Organizations
        [HttpGet]
        public async Task<ActionResult> GetOrganization()
        {
            var _organization = await _context.Organizations.Select(org => new { org.Id, org.ParentId, org.Name, org.Zip, org.Detail, org.Address, org.IsParent, org.Email, org.ContactNumber}).ToListAsync();
            return Ok(_organization);
        }

        //GET : api/Organizations/GetParentOrgs
        [HttpGet]
        [Route("GetParentOrgs")]
        public async Task<ActionResult<OrganizationDTO>> GetParentOrganization()
        {
            List<OrganizationDTO> orgList = await _context.Organizations.Where(org => org.IsParent == true).Select(org => new OrganizationDTO() { Id=org.Id, IsParent=org.IsParent, Name=org.Name }).ToListAsync();

            return Ok(orgList);
        }

        [HttpGet]
        [Route("GetChildOrgs")]
        public async Task<ActionResult<OrganizationDTO>> GetChildOrganization(long parentId)
        {
            List<OrganizationDTO> orgList = await _context.Organizations.Where(org => org.ParentId == parentId).Select(org => new OrganizationDTO() { Id = org.Id, IsParent = org.IsParent, Name = org.Name }).ToListAsync();

            return Ok(orgList);
        }

        // GET: api/Organizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(long id)
        {
            var organization = await _context.Organizations.FindAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return organization;
        }

        // PUT: api/Organizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganization(long id, Organization organization)
        {
            if (id != organization.Id)
            {
                return BadRequest();
            }

            _context.Entry(organization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationExists(id))
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

        // POST: api/Organizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Organization>> PostOrganization(Organization organization)
        {
            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganization", new { id = organization.Id }, organization);
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(long id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }

            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganizationExists(long id)
        {
            return _context.Organizations.Any(e => e.Id == id);
        }
    }
}
