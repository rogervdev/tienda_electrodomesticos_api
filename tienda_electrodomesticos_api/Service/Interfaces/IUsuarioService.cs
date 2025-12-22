using tienda_electrodomesticos_api.Models;

namespace tienda_electrodomesticos_api.Service.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> RegistrarUsuario(Usuario usuario, string password);
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
    }

}
