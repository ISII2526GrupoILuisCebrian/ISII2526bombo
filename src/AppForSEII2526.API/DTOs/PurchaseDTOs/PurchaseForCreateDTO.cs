
﻿using AppForSEII2526.API.Models;
using System.ComponentModel.DataAnnotations;

﻿using System.ComponentModel.DataAnnotations;

using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForCreateDTO
    {
        public PurchaseForCreateDTO(
            string street,
            string city,
            string postalCode,
            string nameCustomer,
            string surnameCustomer,
            int paymentMethodId,
            int? rating,
            IList<PurchaseProductDTO> purchasedProducts)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
            NameCustomer = nameCustomer;
            SurnameCustomer = surnameCustomer;
            PaymentMethodId = paymentMethodId;
            PurchasedProducts = purchasedProducts;

            
        }

        [StringLength(100, MinimumLength = 10, ErrorMessage = "Street must have at least 10 characters")]
        public string Street { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must have at least 3 characters")]
        public string City { get; set; }


        [StringLength(10, MinimumLength = 4, ErrorMessage = "Postal code must have between 4 and 10 characters")]
        public string PostalCode { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have at least 3 characters")]
        public string NameCustomer { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Surname must have at least 3 characters")]
        public string SurnameCustomer { get; set; }

        [Required]

        public IList<PurchaseProductDTO> PurchasedProducts { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

      
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForCreateDTO dTO &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   PostalCode == dTO.PostalCode &&
                   NameCustomer == dTO.NameCustomer &&
                   SurnameCustomer == dTO.SurnameCustomer &&
                   EqualityComparer<IList<PurchaseProductDTO>>.Default.Equals(PurchasedProducts, dTO.PurchasedProducts) &&
                   PaymentMethodId == dTO.PaymentMethodId &&
                   Rating == dTO.Rating;
        }

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

