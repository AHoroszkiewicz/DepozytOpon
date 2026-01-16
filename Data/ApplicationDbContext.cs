using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DepozytOpon.Models;
using System.Threading;
using System.Threading.Tasks;

namespace DepozytOpon.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Opona> Opony { get; set; }
        public DbSet<Depozyt> Depozyt { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Depozyt>()
                .ToTable(tb => tb.HasTrigger("TR_Depozyt_Insert"));

            modelBuilder.Entity<Depozyt>()
                .ToTable(tb => tb.HasTrigger("TR_Depozyt_Delete"));

            modelBuilder.Entity<Depozyt>()
                .HasIndex(d => d.NumerBOX)
                .IsUnique();

            modelBuilder.Entity<Opona>()
               .HasIndex(d => d.KodTowaru)
               .IsUnique();


        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1️⃣ Pobranie zalogowanego użytkownika
            var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";

            // 2️⃣ Ustawienie SESSION_CONTEXT w SQL Server
            if (Database.GetDbConnection() is SqlConnection sqlConn)
            {
                if (sqlConn.State != System.Data.ConnectionState.Open)
                    await sqlConn.OpenAsync(cancellationToken);

                using var command = sqlConn.CreateCommand();
                command.CommandText = @"
                    EXEC sp_set_session_context 
                        @key = N'CurrentUser', 
                        @value = @user";
                command.Parameters.Add(new SqlParameter("@user", user));

                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            // 3️⃣ Wywołanie oryginalnego SaveChangesAsync
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
