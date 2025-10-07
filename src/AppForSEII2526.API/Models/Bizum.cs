namespace AppForSEII2526.API.Models
{
    public class Bizum : PaymentMethod
    {
        [Required]
        [RegularExpression(@"^\+?[0-9]+$", ErrorMessage = "El número de teléfono solo puede contener dígitos y un '+' opcional al inicio.")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "El número de teléfono debe tener entre 7 y 15 dígitos.")]
        [Display(Name = "Telephone Number")]
        public string TelephoneNumber { get; set; }
    }

}
