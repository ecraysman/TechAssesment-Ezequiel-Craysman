using DB.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public partial class AffiliateCodesController : ControllerBase
    {
        

        //Validar que tenemos el affiliate correcto

        // POST: api/AffiliateCodes
        [HttpPost]
        public async Task<ActionResult<AffiliateCodes>> PostAffiliateCodes(AffiliateCodes? affiliateCodes)
        {
            if (_context.AffiliateCodes == null)
            {
                return Problem("Entity set 'AffiliateContext.AffiliateCodes'  is null.");
            }




            bool IsValid = true;
            string ListOfMessages = "";


            //Partial validations


            if (affiliateCodes == null)
            { //Null?
                ListOfMessages += "affiliateCodes info is missing.";
                return Problem(ListOfMessages); //Exit, due to being fatal.
            }
            else //Validate the info
            {
                if (string.IsNullOrEmpty(affiliateCodes.AffiliateCode))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "affiliateCodes Code is missing.";
                }

                //if (string.IsNullOrEmpty(affiliateCodes.Email))
                //{
                //    IsValid = false;
                //    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                //    ListOfMessages += "affiliateCodes mail is missing.";
                //}
                //else
                //{
                //    if (!Strings.IsValidEmail(affiliateCodes.Email))
                //    {
                //        IsValid = false;
                //        if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                //        ListOfMessages += "affiliateCodes mail is invalid.";
                //    }
                //}

                //Id?
                if (!(affiliateCodes.Id == Guid.Empty))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "affiliateCodes id cannot be specified.";
                }



                //Date => No point in validating the date, because it will be NOW regardless of whatever we receive.
                //if (!string.IsNullOrEmpty(affiliateCodes.CreationDate.ToString()))
                //{
                //    IsValid = false;
                //    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                //    ListOfMessages += "Affiliate creation date cannot be specified.";
                //}



                //Affiliate id?
                if (affiliateCodes.AffiliateId == Guid.Empty)
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "AffiliateId MUST be specified.";
                }
                else
                {
                    //Validate its a real and enabled affiliate.
                    var affiliates = _context.Affiliates.FirstOrDefault(x =>  x.Id == affiliateCodes.AffiliateId);
                    if (affiliates == null || affiliates.IsDeleted)
                    {
                        IsValid = false;
                        if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                        ListOfMessages += "AffiliateId is invalid";
                    }

                }


                //Affiliate code, does it belong to someone?
                if (string.IsNullOrEmpty(affiliateCodes.AffiliateCode))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "AffiliateCode  MUST be specified.";
                }
                else
                {
                    //Validate its a real and enabled affiliate.
                    var affiliates =  _context.AffiliateCodes.FirstOrDefault(x => x.AffiliateCode == affiliateCodes.AffiliateCode);
                    if (affiliates != null && !affiliates.IsDeleted)
                    {
                        IsValid = false;
                        if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                        ListOfMessages += "AffiliateCode is duplicated";
                    }

                }

                //Expiration date
                //min 5 days from today
                if(affiliateCodes.ExpirationDate <  DateTime.Now.AddDays(5))
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "ExpirationDate must be at least 5 days in the future";
                }

                //AvailableAmount
                if (affiliateCodes.AvailableAmount < 1)
                {
                    IsValid = false;
                    if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
                    ListOfMessages += "AvailableAmount must be at least 1";
                }



                if (IsValid)
                {
                    //Complete the remainder of information
                    affiliateCodes.Id = Guid.NewGuid();
                    affiliateCodes.CreationDate = DateTime.Now;
                    affiliateCodes.IsDeleted = false;

                    _context.AffiliateCodes.Add(affiliateCodes);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("Get AffiliateCodes", new { id = affiliateCodes.Id, name = nameof(affiliateCodes) });
                }
                else
                {
                    return Problem(ListOfMessages);
                }
            }

        }


    }
}
