<<<<<<< HEAD
﻿using AppForSEII2526.API.DTOs.PurchaseDTOs;

namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseForDetailDTO
    {
        public PurchaseForDetailDTO(int id, decimal totalPrice, DateTime date, string street, string city, string postalCode, string nameSurname, string state, string paymentMethod, string customerUserName, IList<PurchaseProductDTO> products)
        {
            Id = id;
            TotalPrice = totalPrice;
            Date = date;
            Street = street;
            City = city;
            PostalCode = postalCode;
            NameSurname = nameSurname;
            State = state;
            PaymentMethod = paymentMethod;
            CustomerUserName = customerUserName;
            Products = products;
        }

        public PurchaseForDetailDTO(int id, decimal totalPrice, DateTime date, string street, string city, string postalCode, string nameSurname, string state, string paymentMethod, string customerUserName, IList<PurchaseProductDTO> items, int? rating) : this(id, totalPrice, date, street, city, postalCode, nameSurname, state, paymentMethod, customerUserName, items)
        {
            Rating = rating;
=======
﻿using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForDetailDTO : PurchaseForCreateDTO
    {
        public PurchaseForDetailDTO(
            int id,
            DateTime purchaseDate,
            string street,
            string postalCode,
            PurchaseState state,
            decimal totalPrice,
            string nameCustomer,
            string surnameCustomer,
            PaymentMethod paymentMethod,
            string userNameCustomer,
            IList<PurchaseProductDTO> purchaseProducts)

            : base(street, postalCode, nameCustomer, surnameCustomer, purchaseDate, paymentMethod, userNameCustomer, purchaseProducts)
        {
            Id = id;
            TotalPrice = totalPrice;

>>>>>>> origin/development
        }

        public int Id { get; set; }

        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }
<<<<<<< HEAD
        public DateTime Date { get; set; }

        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string NameSurname { get; set; } = default!;

        public string State { get; set; } = default!;
        public string PaymentMethod { get; set; } = default!;
        public string CustomerUserName { get; set; } = default!;
        public IList<PurchaseProductDTO> Products { get; set; } = new List<PurchaseProductDTO>();


        [Range(0, 5)]
        public int? Rating { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForDetailDTO dTO &&
                   Id == dTO.Id &&
                   TotalPrice == dTO.TotalPrice &&
                   Date == dTO.Date &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   PostalCode == dTO.PostalCode &&
                   NameSurname == dTO.NameSurname &&
                   State == dTO.State &&
                   PaymentMethod == dTO.PaymentMethod &&
                   CustomerUserName == dTO.CustomerUserName &&
                   EqualityComparer<IList<PurchaseProductDTO>>.Default.Equals(Products, dTO.Products) &&
                   Rating == dTO.Rating;
        }
    }
}
=======

        public DateTime PurchaseDate { get; set; }



    }
}
>>>>>>> origin/development
