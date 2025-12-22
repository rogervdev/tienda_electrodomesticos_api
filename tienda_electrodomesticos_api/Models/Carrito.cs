using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tienda_electrodomesticos_api.Models
{
    [Table("carrito")]
    public class Carrito
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("producto_id")]
        public int ProductoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Column("cantidad")]
        public int Cantidad { get; set; }

        // 🔹 Para ADO.NET puro, no necesitamos las propiedades de navegación
        // public Usuario? Usuario { get; set; }
        // public Producto? Producto { get; set; }

        // 🔹 Nuevas propiedades para cálculos, no mapeadas
        [NotMapped]
        public double PrecioTotal { get; set; }

        [NotMapped]
        public double TotalPrecioOrdenes { get; set; }
    }
}
