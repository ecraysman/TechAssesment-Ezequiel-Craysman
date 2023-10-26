using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using DB.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AffiliatesController : ControllerBase
    {
        private readonly AffiliateContext _context;

        public AffiliatesController(AffiliateContext context)
        {
            _context = context;
        }

        // GET: api/Affiliates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Affiliates>>> GetAffiliates()
        {
            if (_context.Affiliates == null)
            {
                return NotFound();
            }
            return await _context.Affiliates.Where( x => x.IsDeleted == false).ToListAsync();
        }

        // GET: api/Affiliates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Affiliates>> GetAffiliates(Guid id)
        {
            if (_context.Affiliates == null)
            {
                return NotFound();
            }
            var affiliates = await _context.Affiliates.FindAsync(id);

            if (affiliates == null)
            {
                return NotFound();
            }

            if (affiliates.IsDeleted) return NotFound(); //Once you get deleted, you should never be seen again

            return affiliates;
        }


        // DELETE: api/Affiliates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAffiliates(Guid id)
        {
            if (_context.Affiliates == null)
            {
                return NotFound();
            }
            var affiliates = await _context.Affiliates.FindAsync(id);

            if (affiliates == null)
            {
                return NotFound();
            }
            if(affiliates.IsDeleted) return NoContent(); //Idempotency, 

            affiliates.IsDeleted = true;


            _context.Entry(affiliates).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AffiliatesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            //ToDo: When deleting an affiliate, you need to invalidate all the referral codes

            return NoContent();
        }

        private bool AffiliatesExists(Guid id)
        {
            return (_context.Affiliates?.Any(e => e.Id == id)).GetValueOrDefault();
        }

       
    }
}
