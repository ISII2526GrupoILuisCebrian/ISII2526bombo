using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
<<<<<<< HEAD
    public DbSet<PurchaseProduct> PurchaseProducts { get; set; }
=======
    public DbSet<Product> Products { get; set; }
>>>>>>> origin/development
}
