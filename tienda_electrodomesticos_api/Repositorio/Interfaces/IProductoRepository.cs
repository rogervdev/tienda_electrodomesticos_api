using tienda_electrodomesticos_api.Models;

namespace tienda_electrodomesticos_api.Repositorio.Interfaces
{
    public interface IProductoRepository
    {
        Task<Producto?> ObtenerPorId(int id);
        Task<List<Producto>> ListarTodos();
        Task<List<Producto>> ListarActivos();

        // Métodos opcionales según tus procedimientos
        Task<List<Producto>> ListarPorCategoria(string nombreCategoria);
        Task<List<Producto>> BuscarPorTituloOCategoria(string filtro);

        Task Agregar(Producto producto);
        Task Actualizar(Producto producto);
        Task Eliminar(Producto producto);
        Task<Producto> Guardar(Producto producto); // puede llamar a Agregar internamente
    }


}
