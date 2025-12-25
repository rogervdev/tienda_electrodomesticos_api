using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tienda_electrodomesticos_api.Models.DTOs
{
    public class CategoriaUploadDto
    {
        public string Nombre { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        [FromForm]
        public IFormFile? Imagen { get; set; }
    }
}
