using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;



// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    //Relationships
    public List<Complaint> Complaints { get; set; } // Navigation property to Complaint
    public List<ReportCustomer> ReportCustomers { get; set; } // Navigation property to ReportCustomer
}