using System.Collections.Generic;
using System.Security.Claims;
using tienda_electrodomesticos_api.Models;

namespace tienda_electrodomesticos_api.Config
{
    public class UsuarioPersonalizado
    {
        public Usuario Usuario { get; private set; }

        public UsuarioPersonalizado(Usuario usuario)
        {
            Usuario = usuario;
        }

        /// <summary>
        /// Devuelve claims básicos del usuario. Roles opcionales.
        /// </summary>
        public IEnumerable<Claim> GetClaims(IEnumerable<string>? roles = null)
        {
            // Claim de correo
            yield return new Claim(ClaimTypes.Email, Usuario.Email);

            // Claim de nombre completo
            yield return new Claim(ClaimTypes.Name, $"{Usuario.Nombre} {Usuario.Apellido}");

            // Claims de roles
            if (roles != null)
            {
                foreach (var rol in roles)
                {
                    yield return new Claim(ClaimTypes.Role, rol);
                }
            }
        }

        /// <summary>
        /// Indica si la cuenta está habilitada según is_enable
        /// </summary>
        public bool IsEnabled() => Usuario.IsEnable;
    }
}
