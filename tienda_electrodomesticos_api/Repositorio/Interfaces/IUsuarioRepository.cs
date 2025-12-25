using tienda_electrodomesticos_api.Models;

namespace tienda_electrodomesticos_api.Repositorio.Interfaces
{
        public interface IUsuarioRepository
        {
       
                Task<Usuario?> BuscarPorEmail(string email);
                Task<bool> ExistePorEmail(string email);
                Task<List<Usuario>> ListarPorRol(string rol);
                Task<Usuario?> BuscarPorResetToken(string token);
                Task<Usuario?> ObtenerPorId(int id);

                Task Agregar(Usuario usuario, string password);
                Task Actualizar(Usuario usuario);
                Task Eliminar(int id);
              


    }
}
