using System.ComponentModel.DataAnnotations;

namespace tienda_electrodomesticos_api.Models.DTOs
{
    public class ResetTokenDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string ResetToken { get; set; } = string.Empty;
    }
}
