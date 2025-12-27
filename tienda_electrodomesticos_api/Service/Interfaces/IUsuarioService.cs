using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Models.DTOs;

namespace tienda_electrodomesticos_api.Service.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> RegistrarUsuario(UsuarioRegistrarDto dto);
        Task<Usuario?> ObtenerPorEmail(string email);
        Task<bool> ExistePorEmail(string email);
        Task<List<Usuario>> ListarUsuariosPorRol(string rol);

        Task<bool> ActualizarEstadoCuenta(int id, bool estado);
        Task IncrementarIntentoFallido(Usuario usuario);
        Task BloquearCuenta(Usuario usuario);
        Task<bool> DesbloquearCuentaPorTiempo(Usuario usuario, long unlockDurationMilliseconds);
        Task ReiniciarContador(int usuarioId);

        Task ActualizarResetToken(string email, string resetToken);
        Task<Usuario?> ObtenerPorToken(string token);
        Task<Usuario> ActualizarUsuario(Usuario usuario);

        Task<Usuario?> Login(string email, string password);

    }

}
