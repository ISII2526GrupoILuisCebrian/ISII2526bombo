using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForDetailDTO:PurchaseForCreateDTO
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
            
            :base(street, postalCode, nameCustomer, surnameCustomer, purchaseDate, paymentMethod, userNameCustomer, purchaseProducts)
            {
            Id = id;
            TotalPrice = totalPrice;
            
            }

            public int Id { get; set; }

            [Precision(10, 2)]
            public decimal TotalPrice { get; set; }

            public DateTime PurchaseDate { get; set; }

           
        
    }
}
