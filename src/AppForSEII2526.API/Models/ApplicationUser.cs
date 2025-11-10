using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;


[Index(nameof(Name), nameof(Surname), nameof(Address), nameof(AccountCreationDate), IsUnique = true)]
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ApplicationUser(){
    }

    public ApplicationUser(string userName, DateTime accountCreationDate, string address, string name, string surname, IList<Complaint> complaints, IList<ReportCustomer> reportCustomers, IList<PurchaseOrder> purchaseOrders)
    {
        
        UserName = userName;
        AccountCreationDate = accountCreationDate;
        Address = address;
        Name = name;
        Surname = surname;
        Complaints = complaints;
        ReportCustomers = reportCustomers;
        PurchaseOrders = purchaseOrders;
    }

    public ApplicationUser(string id, string userName, DateTime accountCreationDate, string address, string name, string surname, IList<Complaint> complaints, IList<ReportCustomer> reportCustomers, IList<PurchaseOrder> purchaseOrders)
    {
        Id = id;
        UserName = userName;
        AccountCreationDate = accountCreationDate;
        Address = address;
        Name = name;
        Surname = surname;
        Complaints = complaints;
        ReportCustomers = reportCustomers;
        PurchaseOrders = purchaseOrders;
    }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Account creation date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime AccountCreationDate { get; set; }

    [StringLength(200, ErrorMessage = "Address can be neither longer than 200 characters nor shorter than 10.", MinimumLength = 10)]
    public string Address { get; set; }

    [StringLength(50, ErrorMessage = "Name can be neither longer than 50 characters nor shorter than 10.", MinimumLength = 10)]
    [Required]
    [Key]
    public string Name { get; set; } 

    [StringLength(50, ErrorMessage = "Surname can be neither longer than 50 characters nor shorter than 10.", MinimumLength = 10)]
    public string Surname { get; set; }

    //Relationships
    public IList<Complaint> Complaints { get; set; } // Navigation property to Complaint
    public IList<ReportCustomer> ReportCustomers { get; set; } // Navigation property to ReportCustomer
    public IList<PurchaseOrder> PurchaseOrders { get; set; } // Navigation property to PurchaseOrder


}