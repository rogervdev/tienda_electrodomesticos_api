using Microsoft.AspNetCore.Mvc;

namespace tienda_electrodomesticos_api.Models.DTOs
{
    public class ProductoUploadDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int Descuento { get; set; }
        public decimal PrecioConDescuento { get; set; }
        public bool IsActive { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;

        [FromForm]
        public IFormFile? Imagen { get; set; }
    }

}
