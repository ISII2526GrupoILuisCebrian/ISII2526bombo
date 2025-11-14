namespace AppForSEII2526.API.Models
{
    public class PurchaseOrder
    {
       

        public int Id { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string City { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }


        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Name and surname cannot exceed 150 characters.")]
        public string NameSurname { get; set; }


        [Required]
        [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters.")]
        public string PostalCode { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Street cannot exceed 200 characters.")]
        public string Street { get; set; }

        [Precision(10, 2)]
        [Range(typeof(decimal), "0,00", "9999999999,99")]
        public decimal TotalPrice { get; set; }

        public int PaymentMethodId { get; set; }


        //Relations

        public PurchaseDelivery DriverAssigned { get; set; }

        public List<PurchaseProduct> PurchaseProducts { get; set; } // 1 to N with PurchaseProduct

        public PaymentMethod PaymentMethod { get; set; } // 1 to 1 with PaymentMethod

        public PurchaseState State { get; set; } // 1 to 1 with PurchaseState

        public ApplicationUser Customer { get; set; } // N to 1 with Customer
    }

    public enum PurchaseState
    {
        Request,
        Processing,
        Delivery,
        Done
    }
}
