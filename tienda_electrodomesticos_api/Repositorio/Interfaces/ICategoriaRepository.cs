using tienda_electrodomesticos_api.Models;

namespace tienda_electrodomesticos_api.Repositorio.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> ObtenerPorId(int id);
        Task<List<Categoria>> ListarTodas();
        Task<List<Categoria>> ListarTodasCategorias();
        Task<List<Categoria>> ListarActivas();
        Task Agregar(Categoria categoria);
        Task Actualizar(Categoria categoria);
        Task Eliminar(Categoria categoria);
        Task<bool> ExistePorNombre(string nombre);
    }

}
