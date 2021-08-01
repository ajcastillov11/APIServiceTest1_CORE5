using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceAPITest.Models;

namespace WebServiceAPITest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {
               
        }

        public DbSet<ModelUser> Users { get; set; }
        public DbSet<ModelCategory> Category { get; set; }
        public DbSet<ModelProduct> Products { get; set; }
        
    }
}
