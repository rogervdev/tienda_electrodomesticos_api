using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tienda_electrodomesticos_api.Models
{
    [Table("usuarios")]
    public class Usuario
    {
            [Key]
            [Column("id")]
            public int Id { get; set; }

            [Required, MaxLength(100)]
            [Column("nombre")]
            public string Nombre { get; set; } = string.Empty;

            [Required, MaxLength(100)]
            [Column("apellido")]
            public string Apellido { get; set; } = string.Empty;

            [Required, MaxLength(100)]
            [Column("email")]
            public string Email { get; set; } = string.Empty;

            [Required, MaxLength(255)]
            [Column("passwordhash")]
            public string PasswordHash { get; set; } = string.Empty;

            [MaxLength(150)]
            [Column("profile_image")]
            public string ProfileImage { get; set; } = string.Empty;

            [MaxLength(50)]
            [Column("rol")]
            public string Rol { get; set; } = "ROLE_USER";

            [Column("is_enable")]
            public bool IsEnable { get; set; } = true;

            [Column("cuenta_no_bloqueada")]
            public bool CuentaNoBloqueada { get; set; } = true;

            [Column("intento_fallido")]
            public int IntentoFallido { get; set; }

            [Column("lock_time")]
            public DateTime? LockTime { get; set; }

            [MaxLength(255)]
            [Column("reset_token")]
            public string? ResetToken { get; set; }
        }
    }
