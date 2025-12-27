using tienda_electrodomesticos_api.Models;

namespace tienda_electrodomesticos_api.Service.Interfaces
{
    public interface ICategoriaService
    {
        Task<Categoria> GuardarCategoria(Categoria categoria, IFormFile? imagen = null);
        Task ActualizarCategoria(Categoria categoria, IFormFile? imagen = null);
        Task<bool> ExisteCategoria(string nombre);
        Task<List<Categoria>> GetAllCategorias();

        Task<List<Categoria>> GetAllCategoriasTodas();
        Task<bool> EliminarCategoria(int id);
        Task<Categoria?> ObtenerCategoriaPorId(int id);
        Task<List<Categoria>> GetAllActiveCategorias();
    }


}
