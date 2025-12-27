using tienda_electrodomesticos_api.Models;

namespace tienda_electrodomesticos_api.Service.Interfaces
{
    public interface ICarritoService
    {
        Task<Carrito> GuardarCarrito(int productoId, int usuarioId);
        Task<List<Carrito>> ObtenerCarritoPorUsuario(int usuarioId);
        Task<int> ContarCarrito(int usuarioId);
        Task<bool> EliminarProducto(int usuarioId, int productoId);

        Task<bool> AumentarCantidad(int usuarioId, int productoId);

        Task<bool> DisminuirCantidad(int usuarioId, int productoId);


    }


}
