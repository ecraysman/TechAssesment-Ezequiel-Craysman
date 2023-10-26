using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DB.Models
{
    public class DatabaseContext: DbContext
    {
        public string DbPath { get; }

        public DbSet<Affiliates> Affiliates { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<AffiliateCodes> AffiliateCodes { get; set; }


        public DatabaseContext() 
         {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

}
}
