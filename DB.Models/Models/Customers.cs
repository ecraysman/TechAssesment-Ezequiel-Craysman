using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Models
{
    public class Customers
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }



        public Guid AffiliateId { get; set; }
        public string AffiliateCodeUsed { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }


        public Customers() { 
        
            AffiliateCodeUsed = string.Empty;
            Name = string.Empty;
            Email = string.Empty;

        }
    }
}
