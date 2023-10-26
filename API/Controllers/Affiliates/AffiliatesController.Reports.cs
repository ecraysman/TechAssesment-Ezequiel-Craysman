using DB.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public partial class AffiliatesController : ControllerBase
    {
        //Extra methods

        [HttpGet("{id}/Customers")]
        public async Task<ActionResult<Customers>> GetAffiliateCustomers(Guid id)
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

            //if something is wrong with context.customers (db empty, etc)
            if (_context.Customers == null)
            {
                return NotFound();
            }
            else
            {
                //Filter
                var customers = _context.Customers.AsQueryable();
                customers = customers.Where(x => x.AffiliateId == id).Where(z => z.IsDeleted == false);
                if (customers.Any()) { return Ok(customers.ToList()); }

                return NotFound();
            }
        }
    }
}
