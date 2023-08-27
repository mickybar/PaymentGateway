using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway.Data.Entities
{
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public string Pan { get; set; } = null!;
        [Required]
        public string ExpiryMonth { get; set; } = null!;
        [Required]
        public string ExpiryYear { get; set; } = null!;
        [Required]
        public string Cvv { get; set; } = null!;
    }
}
