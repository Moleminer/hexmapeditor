using Microsoft.EntityFrameworkCore;

namespace HexMapEditor.Data;

public class RGRContext : DbContext
{
    public RGRContext(DbContextOptions<RGRContext> options) : base(options)
    {

    }

    
    // public DbSet<Customer.Models.Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // TODO: Rest of only fluentable stuff here
        // builder.Entity<Transaction>().HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountNumber).OnDelete(DeleteBehavior.Restrict);
        // builder.Entity<Transaction>().HasOne(x => x.DestinationAccount).WithMany().HasForeignKey(x => x.DestinationAccountNumber).OnDelete(DeleteBehavior.Restrict);
        // // builder.Entity<Account>().Property(a => a.Transactions).HasField("_transactions");
        // builder.Entity<Account>().Metadata.FindNavigation(nameof(Account.Transactions)).SetField("_transactions");
    }
    

}