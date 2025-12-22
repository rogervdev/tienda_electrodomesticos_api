using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tienda_electrodomesticos_api.Models
{
    [Table("productos")]
    public class Producto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(500)]
        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Column("categoria_id")]
        public int CategoriaId { get; set; }

        [Required]
        [Column("precio", TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [Column("stock")]
        public int Stock { get; set; }

        [MaxLength(255)]
        [Column("imagen")]
        public string? Imagen { get; set; }

        [Column("descuento")]
        public int Descuento { get; set; }

        [Column("precio_con_descuento", TypeName = "decimal(10,2)")]
        public decimal? PrecioConDescuento { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        
    }
}
