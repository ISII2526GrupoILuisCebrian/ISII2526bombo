namespace AppForSEII2526.API.DTOs.DeliveryDriverDTOs
{
    public class OrderForSchedulingDTO
    {
        public OrderForSchedulingDTO(int id, string street, string city, string postalCode,
                                     DateTime date, decimal totalPrice, string nameSurname)
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
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public string NameSurname { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is OrderForSchedulingDTO dto &&
                   Id == dto.Id &&
                   Street == dto.Street &&
                   City == dto.City &&
                   PostalCode == dto.PostalCode &&
                   Date == dto.Date &&
                   TotalPrice == dto.TotalPrice &&
                   NameSurname == dto.NameSurname;
        }
    }
}
