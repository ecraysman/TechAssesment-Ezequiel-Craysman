using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using DB.Models;
using Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CustomersController : ControllerBase
    {
        private readonly AffiliateContext _context;

        public CustomersController(AffiliateContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            return await _context.Customers.Where(x => x.IsDeleted == false).ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetCustomers(Guid id)
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            var customers = await _context.Customers.FindAsync(id);

            if (customers == null )
            {
                return NotFound();
            }
            if(customers.IsDeleted == true) { return NotFound(); }

            return customers;
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomers(Guid id, Customers customers)
        {
            if (id != customers.Id)
            {
                return BadRequest();
            }

            _context.Entry(customers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersExists(id))
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

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customers>> PostCustomers(Customers? customers)
        {
          if (_context.Customers == null)
          {
              return Problem("Entity set 'AffiliateContext.Customers'  is null.");
          }


            bool IsValid = true;
            string ListOfMessages = "";



            //Validations
            //Code
            //IdAffiliate

            if (customers == null)
            { //Null?
                ListOfMessages += "Customers info is missing.";
                return Problem(ListOfMessages); //Exit, due to being fatal.
            }
            else //Validate the info
            {
                if (string.IsNullOrEmpty(customers.Name))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "Customers name is missing.";
                }

                if (string.IsNullOrEmpty(customers.Email))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "Customers mail is missing.";
                }
                else
                {
                    if (!Strings.IsValidEmail(customers.Email))
                    {
                        IsValid = false;
                        if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                        ListOfMessages += "Customers mail is invalid.";
                    }
                }

                //Id?
                if (!(customers.Id == Guid.Empty))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "Customers id cannot be specified.";
                }

                //Date => No point in validating the date, because it will be NOW regardless of whatever we receive.
                //if (!string.IsNullOrEmpty(customers.CreationDate.ToString()))
                //{
                //    IsValid = false;
                //    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                //    ListOfMessages += "Affiliate creation date cannot be specified.";
                //}



                //Affiliate id?
                if (customers.AffiliateId == Guid.Empty)
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "AffiliateId MUST be specified.";
                }
                else
                {
                    //Validate its a real and enabled affiliate.
                    var affiliates = await _context.Affiliates.FindAsync(customers.AffiliateId);
                    if(affiliates == null || affiliates.IsDeleted)
                    {
                        IsValid = false;
                        if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                        ListOfMessages += "AffiliateId is invalid";
                    }
                    
                }


                //Affiliate code, does it belong to someone?
                if (string.IsNullOrEmpty(customers.AffiliateCodeUsed))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "AffiliateCodeUsed MUST be specified.";
                }
                else
                {
                    //Validate its a real and enabled affiliate.
                    var affiliates = await _context.AffiliateCodes.FirstOrDefaultAsync(x => x.AffiliateCode == customers.AffiliateCodeUsed);
                    if (affiliates == null || affiliates.IsDeleted || affiliates.IsCompleted || affiliates.AvailableAmount < 1 || affiliates.ExpirationDate < DateTime.Now)
                    {
                        IsValid = false;
                        if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                        ListOfMessages += "AffiliateCodeUsed is invalid";
                    }
                    else
                    {
                        //Since it was valid, we mark as used
                        
                        
                        //var TempCodesController = new AffiliateCodesController(_context);


                        if(Utils.AffiliateCodes.MarkAsUsed(affiliates.Id, _context) == -1)
                        {
                            IsValid = false;
                            if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                            ListOfMessages += "AffiliateCodeUsed is invalid";

                        }
                    }



                }




                if (IsValid)
                {
                    //Complete the remainder of information
                    customers.Id = Guid.NewGuid();
                    customers.CreationDate = DateTime.Now;
                    customers.IsDeleted = false;

                    _context.Customers.Add(customers);
                    await _context.SaveChangesAsync();





                    return CreatedAtAction("Get Customers", new { id = customers.Id, name = nameof(customers) });
                }
                else
                {
                    return Problem(ListOfMessages);
                }
            }
        }




        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomers(Guid id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }

            if (customers.IsDeleted) return NoContent(); //Idempotency validation

            customers.IsDeleted = true;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersExists(id))
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

        private bool CustomersExists(Guid id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
