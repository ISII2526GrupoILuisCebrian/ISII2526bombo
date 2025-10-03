namespace AppForSEII2526.API.Models
{
    public class Complaint
    {

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Complaint Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ComplaintDate { get; set; }

        [StringLength(800, ErrorMessage = "Description can be neither longer than 800 characters nor shorter than 10", MinimumLength =10)]
        public string Description { get; set; }


        public int Id { get; set; }


        public bool Processed { get; set; }

        //Relationships
        //public Customer Customers { get; set; } // Navigation property to Customer. I DON'T KNOW WHAT TO INTERPRET WITH 'CUSTOMER'

        public ComplaintType ComplaintType { get; set; } // Navigation property to ComplaintType
    }
}
