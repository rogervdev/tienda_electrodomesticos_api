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
            // Guardar imagen si viene
            if (imagen != null && imagen.Length > 0)
            {
                var fileName = Path.GetFileName(imagen.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productos", fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imagen.CopyToAsync(stream);

                producto.Imagen = fileName;
            }

            // Guardar producto usando el repositorio / SP
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

            // Borrar la imagen física si existe
            if (!string.IsNullOrEmpty(producto.Imagen))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productos", producto.Imagen);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            // Borrar el registro de la base de datos
            await _productoRepository.Eliminar(producto);
            return true;
        }


        public async Task<Producto?> GetProductoById(int id)
        {
            return await _productoRepository.ObtenerPorId(id);
        }

        public async Task<Producto?> ActualizarProducto(Producto producto, IFormFile? imagen = null)
        {
            // 1️⃣ Obtener producto actual
            var prodDb = await _productoRepository.ObtenerPorId(producto.Id);
            if (prodDb == null) return null;

            // 2️⃣ Actualizar datos
            prodDb.Titulo = producto.Titulo;
            prodDb.Descripcion = producto.Descripcion;
            prodDb.CategoriaId = producto.CategoriaId;
            prodDb.Precio = producto.Precio;
            prodDb.Stock = producto.Stock;
            prodDb.Descuento = producto.Descuento;
            prodDb.IsActive = producto.IsActive;

            // 3️⃣ SOLO cambiar imagen si hay nueva
            if (imagen != null && imagen.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imagen.FileName);
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/productos",
                    fileName
                );

                using var stream = new FileStream(filePath, FileMode.Create);
                await imagen.CopyToAsync(stream);

                prodDb.Imagen = fileName;
            }
            // ❌ NO tocar prodDb.Imagen si no hay imagen

            // 4️⃣ Guardar
            await _productoRepository.Actualizar(prodDb);
            return prodDb;
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

        private decimal CalcularPrecioConDescuento(decimal precio, int descuento)
        {
            if (descuento <= 0) return precio;
            if (descuento > 100) descuento = 100;

            return Math.Round(
                precio - (precio * descuento / 100m),
                2
            );
        }

    }
}
