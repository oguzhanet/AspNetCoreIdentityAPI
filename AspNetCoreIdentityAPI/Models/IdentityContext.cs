using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityAPI.Models
{
    public class IdentityContext: IdentityDbContext<AppUser>
    {

        public IdentityContext(DbContextOptions<IdentityContext> options):base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-908ENCT;Database=IdentityApiDb;Trusted_Connection=true");
        }
    }
}
