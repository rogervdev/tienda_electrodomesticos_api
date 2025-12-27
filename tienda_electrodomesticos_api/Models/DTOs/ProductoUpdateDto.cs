using Microsoft.AspNetCore.Http;

namespace tienda_electrodomesticos_api.Models.DTOs
{
    public class ProductoUpdateDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int Descuento { get; set; }
        public bool IsActive { get; set; }

        // 👇 imagen existente
        public string? Imagen { get; set; }

        // 👇 imagen nueva
        public IFormFile? ImagenFile { get; set; }
    }
}

