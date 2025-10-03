using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
       public DbSet<Brand> Brands { get; set; }

    public DbSet<PurchaseProduct> PurchaseProducts { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ReportCustomer> ReportCustomers { get; set; }

    public DbSet<BanReport> BanReports { get; set; }

    public DbSet<ComplaintType> ComplaintTypes { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    

    public DbSet<Complaint> Complaints { get; set; }


    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}
