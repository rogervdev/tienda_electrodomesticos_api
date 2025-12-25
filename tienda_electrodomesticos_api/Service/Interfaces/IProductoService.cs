namespace tienda_electrodomesticos_api.Service.Interfaces
{
    using Microsoft.AspNetCore.Http;
    using tienda_electrodomesticos_api.Models;

    public interface IProductoService
    {
        // En la interfaz
        Task<Producto> GuardarProducto(Producto producto, IFormFile? imagen = null);
        Task<List<Producto>> GetAllProductos();
        Task<bool> EliminarProducto(int id);
        Task<Producto?> GetProductoById(int id);
        Task<Producto?> ActualizarProducto(Producto producto, IFormFile? imagen = null);

        Task<List<Producto>> GetAllActiveProductos(string? categoria = null);
        Task<List<Producto>> BuscarProducto(string filtro);


    }

}
