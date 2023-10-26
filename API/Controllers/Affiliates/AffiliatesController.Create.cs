using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace API.Controllers
{
    public partial class AffiliatesController : ControllerBase
    {
        // POST: api/Affiliates
        [HttpPost]
        public async Task<ActionResult<Affiliates>> PostAffiliates(Affiliates? affiliates)
        {
            if (_context.Affiliates == null)
            {
                return Problem("Entity set 'AffiliateContext.Affiliates'  is null.");
            }

            bool IsValid = true;
            string ListOfMessages = "";

            if (affiliates == null)
            { //Null?
                ListOfMessages += "Affiliate info is missing.";
                return Problem(ListOfMessages); //Exit, due to being fatal.
            }
            else //Validate the info
            {
                if (string.IsNullOrEmpty(affiliates.Name))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "Affiliate name is missing.";
                }

                if (string.IsNullOrEmpty(affiliates.Email))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "Affiliate mail is missing.";
                }
                else
                {
                    if (!Strings.IsValidEmail(affiliates.Email))
                    {
                        IsValid = false;
                        if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                        ListOfMessages += "Affiliate mail is invalid.";
                    }
                }

                //Id?
                if (!(affiliates.Id == Guid.Empty))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "Affiliate id cannot be specified.";
                }

                //Date => No point in validating the date, because it will be NOW regardless of whatever we receive.
                //if (!string.IsNullOrEmpty(affiliates.CreationDate.ToString()))
                //{
                //    IsValid = false;
                //    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                //    ListOfMessages += "Affiliate creation date cannot be specified.";
                //}

                if (IsValid)
                {
                    //Complete the remainder of information
                    affiliates.Id = Guid.NewGuid();
                    affiliates.CreationDate = DateTime.Now;
                    affiliates.IsDeleted = false;

                    _context.Affiliates.Add(affiliates);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetAffiliates", new { id = affiliates.Id, name = nameof(affiliates) });
                }
                else
                {
                    return Problem(ListOfMessages);
                }
            }
        }
    }
}
