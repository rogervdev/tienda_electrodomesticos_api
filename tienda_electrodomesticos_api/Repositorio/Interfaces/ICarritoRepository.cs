using tienda_electrodomesticos_api.Models;

namespace tienda_electrodomesticos_api.Repositorio.Interfaces
{
    public interface ICarritoRepository
    {
        Task<Carrito?> BuscarPorProductoYUsuario(int productoId, int usuarioId);
        Task<int> ContarPorUsuario(int usuarioId);
        Task<List<Carrito>> ListarPorUsuario(int usuarioId);
        Task Guardar(Carrito carrito);
        Task Eliminar(Carrito carrito);

    }


}
