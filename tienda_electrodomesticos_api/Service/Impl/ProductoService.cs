using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Repositorio.Interfaces;
using tienda_electrodomesticos_api.Service.Interfaces;

namespace tienda_electrodomesticos_api.Service.Impl
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<Producto> GuardarProducto(Producto producto, IFormFile? imagen = null)
        {
            if (imagen != null && imagen.Length > 0)
            {
                using var ms = new MemoryStream();
                await imagen.CopyToAsync(ms);
                // Aquí solo guardamos el nombre, el archivo puedes guardarlo en disco o nube según tu lógica
                producto.Imagen = imagen.FileName;
            }

            return await _productoRepository.Guardar(producto);
        }

        public async Task<List<Producto>> GetAllProductos()
        {
            return await _productoRepository.ListarTodos();
        }

        public async Task<bool> EliminarProducto(int id)
        {
            var producto = await _productoRepository.ObtenerPorId(id);
            if (producto == null) return false;

            await _productoRepository.Eliminar(producto);
            return true;
        }

        public async Task<Producto?> GetProductoById(int id)
        {
            return await _productoRepository.ObtenerPorId(id);
        }

        public async Task<Producto?> ActualizarProducto(Producto producto, IFormFile? imagen = null)
        {
            if (imagen != null && imagen.Length > 0)
            {
                using var ms = new MemoryStream();
                await imagen.CopyToAsync(ms);
                producto.Imagen = imagen.FileName;
            }

            await _productoRepository.Actualizar(producto);
            return producto;
        }

        public async Task<List<Producto>> GetAllActiveProductos(string? categoria = null)
        {
            if (!string.IsNullOrEmpty(categoria))
            {
                return await _productoRepository.ListarPorCategoria(categoria);
            }
            return await _productoRepository.ListarActivos();
        }

        public async Task<List<Producto>> BuscarProducto(string filtro)
        {
            return await _productoRepository.BuscarPorTituloOCategoria(filtro);
        }
    }
}
