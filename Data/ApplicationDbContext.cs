using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DepozytOpon.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DepozytOpon.Models.Opona> Opony { get; set; }
        public DbSet<DepozytOpon.Models.Depozyt> Depozyty { get; set; }
    }
}
