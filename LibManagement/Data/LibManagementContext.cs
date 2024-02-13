using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibManagement.Data
{
    public class LibManagementContext : IdentityDbContext<Member>
    {
        public LibManagementContext (DbContextOptions<LibManagementContext> options)
            : base(options)
        {
        }
        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }*/

        public DbSet<LibManagement.Models.Book> Book { get; set; } = default!;
        public DbSet<LibManagement.Models.Member> Member { get; set; } = default!;
        public DbSet<LibManagement.Models.Fine> Fine { get; set; } = default!;
    }
}
