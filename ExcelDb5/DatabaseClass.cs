using ExcelDb5.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelDb5
{
    public class DatabaseClass : DbContext
    {
        public DatabaseClass(DbContextOptions<DatabaseClass> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }


    }

}
