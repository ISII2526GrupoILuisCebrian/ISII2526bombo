namespace AppForSEII2526.API.DTOs.DeliveryDriverDTOs
{
    public class OrderForSchedulingDTO
    {
        public OrderForSchedulingDTO(int id, string street, string city, string postalCode, DateTime date, decimal totalPrice, string nameSurname)
        {
            Id = id;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Date = date;
            TotalPrice = totalPrice;
            NameSurname = nameSurname;
        }

        public int Id { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public DateTime Date { get; set; }

        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public string NameSurname { get; set; }
    }
}
