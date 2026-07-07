using Microsoft.EntityFrameworkCore;
using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Family> Families { get; set; }
    public DbSet<FamilyMember> FamilyMembers { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<HdfcBankTransaction> HdfcBankTransactions { get; set; }
    public DbSet<IciciBankTransaction> IciciBankTransactions { get; set; }
    public DbSet<KotakBankTransaction> KotakBankTransactions { get; set; }
    public DbSet<YesBankTransaction> YesBankTransactions { get; set; }
    public DbSet<TransactionCategory> TransactionCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Family configuration
        modelBuilder.Entity<Family>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.HasMany(e => e.Members)
                .WithOne(e => e.Family)
                .HasForeignKey(e => e.FamilyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // FamilyMember configuration
        modelBuilder.Entity<FamilyMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Relationship).HasMaxLength(100);
            entity.HasMany(e => e.Accounts)
                .WithOne(e => e.Member)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // BankAccount configuration
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BankName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AccountType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AccountHolder).HasMaxLength(200);
            entity.Property(e => e.CurrentBalance).HasPrecision(18, 2);
            entity.Property(e => e.OpeningBalance).HasPrecision(18, 2);
        });

        // HdfcBankTransaction configuration
        modelBuilder.Entity<HdfcBankTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("HdfcBankTransactions");
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Balance).HasPrecision(18, 2);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        // IciciBankTransaction configuration
        modelBuilder.Entity<IciciBankTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("IciciBankTransactions");
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Balance).HasPrecision(18, 2);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        // KotakBankTransaction configuration
        modelBuilder.Entity<KotakBankTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("KotakBankTransactions");
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Balance).HasPrecision(18, 2);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        // YesBankTransaction configuration
        modelBuilder.Entity<YesBankTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("YesBankTransactions");
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Balance).HasPrecision(18, 2);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        // TransactionCategory configuration
        modelBuilder.Entity<TransactionCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // Seed default categories
        SeedCategories(modelBuilder);
    }

    private void SeedCategories(ModelBuilder modelBuilder)
    {
        var categories = new[]
        {
            new TransactionCategory { Id = 1, Name = "Salary", Description = "Salary and wages" },
            new TransactionCategory { Id = 2, Name = "Transfer", Description = "Fund transfers" },
            new TransactionCategory { Id = 3, Name = "Bills & Utilities", Description = "Utility payments" },
            new TransactionCategory { Id = 4, Name = "Groceries", Description = "Grocery shopping" },
            new TransactionCategory { Id = 5, Name = "Entertainment", Description = "Entertainment expenses" },
            new TransactionCategory { Id = 6, Name = "Travel", Description = "Travel and transportation" },
            new TransactionCategory { Id = 7, Name = "Shopping", Description = "Shopping and retail" },
            new TransactionCategory { Id = 8, Name = "Medical", Description = "Medical expenses" },
            new TransactionCategory { Id = 9, Name = "Education", Description = "Education related expenses" },
            new TransactionCategory { Id = 10, Name = "Investment", Description = "Investment transactions" },
            new TransactionCategory { Id = 11, Name = "Insurance", Description = "Insurance payments" },
            new TransactionCategory { Id = 12, Name = "Others", Description = "Other transactions" }
        };

        modelBuilder.Entity<TransactionCategory>().HasData(categories);
    }
}
