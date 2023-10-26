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
    public partial class AffiliateCodesController : ControllerBase
    {
        private readonly AffiliateContext _context;

        public AffiliateCodesController(AffiliateContext context)
        {
            _context = context;
        }

        // GET: api/AffiliateCodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AffiliateCodes>>> GetAffiliateCodes()
        {
          if (_context.AffiliateCodes == null)
          {
              return NotFound();
          }
            return await _context.AffiliateCodes.Where(x => x.IsDeleted == false).ToListAsync();
        }

        // GET: api/AffiliateCodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AffiliateCodes>> GetAffiliateCodes(Guid id)
        {
          if (_context.AffiliateCodes == null)
          {
              return NotFound();
          }
            var affiliateCodes = await _context.AffiliateCodes.FindAsync(id);

            if (affiliateCodes == null)
            {
                return NotFound();
            }

            if(affiliateCodes.IsDeleted) return NotFound();

            return affiliateCodes;
        }



        //Codes are not editable by the end users. 

        //// PUT: api/AffiliateCodes/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAffiliateCodes(Guid id, AffiliateCodes affiliateCodes)
        //{
        //    if (id != affiliateCodes.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(affiliateCodes).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AffiliateCodesExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}



        ////Validar que tenemos el affiliate correcto

        //// POST: api/AffiliateCodes
        //[HttpPost]
        //public async Task<ActionResult<AffiliateCodes>> PostAffiliateCodes(AffiliateCodes affiliateCodes)
        //{
        //    if (_context.AffiliateCodes == null)
        //    {
        //        return Problem("Entity set 'AffiliateContext.AffiliateCodes'  is null.");
        //    }
        //    _context.AffiliateCodes.Add(affiliateCodes);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAffiliateCodes", new { id = affiliateCodes.Id }, affiliateCodes);
        //}

        // DELETE: api/AffiliateCodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAffiliateCodes(Guid id)
        {
            if (_context.AffiliateCodes == null)
            {
                return NotFound();
            }
            var affiliateCodes = await _context.AffiliateCodes.FindAsync(id);
            if (affiliateCodes == null)
            {
                return NotFound();
            }

            if(affiliateCodes.IsDeleted) { return NoContent(); }

            affiliateCodes.IsDeleted = true;


            _context.Entry(affiliateCodes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AffiliateCodesExists(id))
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

        private bool AffiliateCodesExists(Guid id)
        {
            return (_context.AffiliateCodes?.Any(e => e.Id == id)).GetValueOrDefault();
        }


   
    }
}
