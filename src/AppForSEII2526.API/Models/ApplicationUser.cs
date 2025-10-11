using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;


[Index(nameof(Name), nameof(Surname), nameof(Address), nameof(AccountCreationDate), IsUnique = true)]
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{

    [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Account creation date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime AccountCreationDate { get; set; }

    [StringLength(200, ErrorMessage = "Address can be neither longer than 200 characters nor shorter than 10.", MinimumLength = 10)]
    public string Address { get; set; }

    [StringLength(50, ErrorMessage = "Name can be neither longer than 50 characters nor shorter than 10.", MinimumLength = 10)]
    [Key]
    public string Name { get; set; } // HOW IS THE 'REQUIRED, NOT NULL' THING WORL HERE

    [StringLength(50, ErrorMessage = "Surname can be neither longer than 50 characters nor shorter than 10.", MinimumLength = 10)]
    public string Surname { get; set; }

    //Relationships
    public List<Complaint> Complaints { get; set; } // Navigation property to Complaint
    public List<ReportCustomer> ReportCustomers { get; set; } // Navigation property to ReportCustomer
    public List<PurchaseOrder> PurchaseOrders { get; set; } // Navigation property to PurchaseOrder


}