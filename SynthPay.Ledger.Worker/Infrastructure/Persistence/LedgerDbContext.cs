using Microsoft.EntityFrameworkCore;
using SynthPay.Ledger.Worker.Domain.Entities;

namespace SynthPay.Ledger.Worker.Infrastructure.Persistence
{
    public class LedgerDbContext : DbContext
    {
        public LedgerDbContext(DbContextOptions<LedgerDbContext> options) : base(options) { }

        public DbSet<AccountBalance> Balances { get; set; }
        public DbSet<ProcessedTransaction> ProcessedTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountBalance>(entity =>
            {
                entity.HasKey(b => b.AccountId);
                entity.Property(b => b.CurrentBalance).HasColumnType("decimal(18,2)").IsRequired();
            });

            modelBuilder.Entity<ProcessedTransaction>(entity =>
            {
                entity.HasKey(p => p.TransactionId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
