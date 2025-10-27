using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForCreateDTO
    {
        public PurchaseForCreateDTO(
            string street,
            string postalCode,
            string nameCustomer,
            string surnameCustomer,
            DateTime purchaseDate,
            PaymentMethod paymentMethod,
            string userNameCustomer,
            IList<PurchaseProductDTO> purchaseProducts)
            {
                Street = street;
                PostalCode = postalCode;
                NameCustomer = nameCustomer;
                SurnameCustomer = surnameCustomer;
                PurchaseDate = purchaseDate;
                PaymentMethod = paymentMethod;
                UserNameCustomer = userNameCustomer;
                PurchaseProducts = purchaseProducts;
            }

        [StringLength(100, MinimumLength = 10, ErrorMessage = "Street must have at least 10 characters")]
        public string Street { get; set; }

        [StringLength(10, MinimumLength = 4, ErrorMessage = "Postal code must have between 4 and 10 characters")]
        public string PostalCode { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have at least 3 characters")]
        public string NameCustomer { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Surname must have at least 3 characters")]
        public string SurnameCustomer { get; set; }

        [Required]
        public IList<PurchaseProductDTO> PurchaseProducts { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [EmailAddress]
        [Required]
        public string UserNameCustomer { get; set; }

    }
}

