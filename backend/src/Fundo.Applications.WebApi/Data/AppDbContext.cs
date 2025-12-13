using Fundo.Applications.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Applications.WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Loan> Loans => Set<Loan> ();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Loan>(e =>
            {
                 e.HasKey(x => x.Id);

                e.Property(x => x.Amount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                e.Property(x => x.CurrentBalance)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                e.Property(x => x.ApplicantName)
                    .HasColumnType("text")
                    .IsRequired();

                e.Property(x => x.Status).IsRequired();
                e.Property(x => x.CreatedAt).IsRequired();
            });
        }
    }
}
