using Microsoft.EntityFrameworkCore;

namespace Erp.Api;
public sealed class ErpDbContext(DbContextOptions<ErpDbContext> options) : DbContext(options)
{
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<ErpUser> Users => Set<ErpUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserBusinessAssignment> UserBusinessAssignments => Set<UserBusinessAssignment>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceLine> SalesInvoiceLines => Set<SalesInvoiceLine>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalLine> JournalLines => Set<JournalLine>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Business>().HasIndex(x => x.Code).IsUnique();
        m.Entity<RolePermission>().HasKey(x => new { x.RoleId, x.PermissionId });
        m.Entity<UserBusinessAssignment>().HasKey(x => new { x.UserId, x.BusinessId });
        m.Entity<ErpUser>().HasIndex(x => x.Email).IsUnique();
        m.Entity<Role>().HasIndex(x => x.Name).IsUnique();
        m.Entity<Permission>().HasIndex(x => x.Key).IsUnique();
        m.Entity<Product>().HasIndex(x => new { x.BusinessId, x.Sku }).IsUnique();
        m.Entity<StockMovement>().HasIndex(x => new { x.BusinessId, x.ProductId, x.TimestampUtc });
        m.Entity<SalesInvoice>().HasIndex(x => new { x.BusinessId, x.Number }).IsUnique();
        m.Entity<SalesInvoiceLine>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        m.Entity<SalesInvoiceLine>().Property(x => x.Quantity).HasPrecision(18, 3);
        m.Entity<JournalLine>().Property(x => x.Debit).HasPrecision(18, 2);
        m.Entity<JournalLine>().Property(x => x.Credit).HasPrecision(18, 2);
        m.Entity<JournalLine>().HasCheckConstraint("CK_JournalLine_Nonnegative", "\"Debit\" >= 0 AND \"Credit\" >= 0");
    }
}
public sealed class Business { public Guid Id { get; set; } = Guid.NewGuid(); public string Code { get; set; } = ""; public string Name { get; set; } = ""; }
public sealed class ErpUser { public Guid Id { get; set; } = Guid.NewGuid(); public string Email { get; set; } = ""; public Guid? RoleId { get; set; } }
public sealed class Role { public Guid Id { get; set; } = Guid.NewGuid(); public string Name { get; set; } = ""; }
public sealed class Permission { public Guid Id { get; set; } = Guid.NewGuid(); public string Key { get; set; } = ""; }
public sealed class RolePermission { public Guid RoleId { get; set; } public Guid PermissionId { get; set; } }
public sealed class UserBusinessAssignment { public Guid UserId { get; set; } public Guid BusinessId { get; set; } }
public sealed class Product { public Guid Id { get; set; } = Guid.NewGuid(); public Guid BusinessId { get; set; } public string Sku { get; set; } = ""; public string Name { get; set; } = ""; }
public sealed class StockMovement { public Guid Id { get; set; } = Guid.NewGuid(); public Guid BusinessId { get; set; } public Guid ProductId { get; set; } public decimal QuantityDelta { get; set; } public DateTimeOffset TimestampUtc { get; set; } = DateTimeOffset.UtcNow; }
public sealed class SalesInvoice { public Guid Id { get; set; } = Guid.NewGuid(); public Guid BusinessId { get; set; } public string Number { get; set; } = ""; public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow; }
public sealed class SalesInvoiceLine { public Guid Id { get; set; } = Guid.NewGuid(); public Guid BusinessId { get; set; } public Guid SalesInvoiceId { get; set; } public Guid ProductId { get; set; } public decimal Quantity { get; set; } public decimal UnitPrice { get; set; } }
public sealed class JournalEntry { public Guid Id { get; set; } = Guid.NewGuid(); public Guid BusinessId { get; set; } public string Reference { get; set; } = ""; public DateTimeOffset PostedUtc { get; set; } = DateTimeOffset.UtcNow; }
public sealed class JournalLine { public Guid Id { get; set; } = Guid.NewGuid(); public Guid BusinessId { get; set; } public Guid JournalEntryId { get; set; } public string AccountCode { get; set; } = ""; public decimal Debit { get; set; } public decimal Credit { get; set; } }
