using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {


    public DbSet<Brand> Brands { get; set; }

    public DbSet<PurchaseDelivery> PurchaseDeliveries { get; set; }
    


    public DbSet<PurchaseProduct> PurchaseProducts { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ReportCustomer> ReportCustomers { get; set; }

    public DbSet<BanReport> BanReports { get; set; }

    public DbSet<ComplaintType> ComplaintTypes { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    
    public DbSet<Complaint> Complaints { get; set; }

    public DbSet<Bizum> Bizums { get; set; }

    public DbSet<PayPal> PayPals { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PaymentMethod>()
            .HasDiscriminator<string>("PaymentMethodType")
            .HasValue<PayPal>("PayPal")
            .HasValue<Bizum>("Bizum")
            .HasValue<CreditCard>("CreditCard");
    }



    public DbSet<DeliveryDriver> DeliveryDrivers { get; set; }
    public DbSet<DeliveryAssignment> DeliveryAssignments { get; set; }

}
