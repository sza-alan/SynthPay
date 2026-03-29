using Microsoft.EntityFrameworkCore;
using SynthPay.Ledger.Worker.Domain.Entities;

namespace SynthPay.Ledger.Worker.Infrastructure.Persistence
{
    public class LedgerDbContext : DbContext
    {
        public LedgerDbContext(DbContextOptions<LedgerDbContext> options) : base(options) { }

        public DbSet<AccountBalance> Balances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountBalance>(entity =>
            {
                entity.HasKey(b => b.AccountId);

                entity.Property(b => b.CurrentBalance)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
