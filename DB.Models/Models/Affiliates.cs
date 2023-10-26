using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace DB.Models
{
    public class Affiliates
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationDate { get; set; }


        public Affiliates() {
            Name = string.Empty;
            Email = string.Empty;
        }

    }
}
