using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Utils
{

    public class AffiliateCodes
    {
        public static int MarkAsUsed(Guid id, DatabaseContext _context, int Qty = 1)
        {
            var CurrentCode = (_context.AffiliateCodes?.FirstOrDefault(e => e.Id == id)) ?? throw new Exception("AffiliateCode Not found");
            
            CurrentCode.AvailableAmount -= Qty;
            if (CurrentCode.AvailableAmount == 0) CurrentCode.IsCompleted = true;
            if (CurrentCode.AvailableAmount < 0) return -1;


            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return CurrentCode.AvailableAmount;
        }
    }


}