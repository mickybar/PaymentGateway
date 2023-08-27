using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Common.Models
{
    public class CardDetails
    {
        [Required] [StringLength(16)] public string Pan { get; set; } = null!;
        
        [Required]
        [MaxLength(2)]
        public string ExpiryMonth { get; set; } = null!;

        [Required]
        [MaxLength(2)]
        public string ExpiryYear { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(4)]
        public string Cvv { get; set; } = null!;

    }
}
