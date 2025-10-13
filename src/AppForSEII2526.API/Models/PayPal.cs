namespace AppForSEII2526.API.Models
{
    public class PayPal : PaymentMethod
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
    }
}
