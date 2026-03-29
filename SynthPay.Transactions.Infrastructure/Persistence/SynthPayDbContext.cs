using Microsoft.EntityFrameworkCore;
using SynthPay.Transactions.Domain.Entities;
using SynthPay.Transactions.Infrastructure.Outbox;

namespace SynthPay.Transactions.Infrastructure.Persistence
{
    public class SynthPayDbContext : DbContext
    {
        public SynthPayDbContext(DbContextOptions<SynthPayDbContext> options) : base(options) { }
        
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.AccountId).IsRequired();
                entity.Property(t => t.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(t => t.Type).IsRequired();
                entity.Property(t => t.Status).IsRequired();
            });

            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.HasKey(o => o.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
