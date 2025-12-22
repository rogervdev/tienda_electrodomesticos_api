using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tienda_electrodomesticos_api.Models
{
    [Table("categorias")]
    public class Categoria
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("imagen_nombre")]
        public string? ImagenNombre { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        
    }
}
