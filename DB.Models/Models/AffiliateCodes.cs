using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Models
{
    public class AffiliateCodes
    {
   

        public Guid Id { get; set; }
        public Guid AffiliateId { get; set; }
        public string AffiliateCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }


        public int AvailableAmount { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; }

        public AffiliateCodes()
        {
            AvailableAmount = 5;
            IsDeleted = false;
            IsCompleted = false;
            AffiliateCode = string.Empty;
        }



    }
}
