using System.Collections.Generic;
using System.Threading.Tasks;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Repositorio.Interfaces;
using tienda_electrodomesticos_api.Service.Interfaces;
namespace tienda_electrodomesticos_api.Service.Impl
{


    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IProductoRepository _productoRepo;

        public CarritoService(
            ICarritoRepository carritoRepo,
            IUsuarioRepository usuarioRepo,
            IProductoRepository productoRepo)
        {
            _carritoRepo = carritoRepo;
            _usuarioRepo = usuarioRepo;
            _productoRepo = productoRepo;
        }

        public async Task<Carrito> GuardarCarrito(int productoId, int usuarioId)
        {
            var usuario = await _usuarioRepo.ObtenerPorId(usuarioId)
                ?? throw new Exception("Usuario no existe");

            var producto = await _productoRepo.ObtenerPorId(productoId)
                ?? throw new Exception("Producto no existe");

            var carrito = await _carritoRepo
                .BuscarPorProductoYUsuario(productoId, usuarioId);

            if (carrito == null)
            {
                carrito = new Carrito
                {
                    UsuarioId = usuarioId,
                    ProductoId = productoId,
                    Cantidad = 1
                };
            }
            else
            {
                carrito.Cantidad++;
            }

            var precio = (double)(producto.PrecioConDescuento ?? 0m);
            carrito.PrecioTotal = carrito.Cantidad * precio;

            await _carritoRepo.Guardar(carrito);
            return carrito;
        }

        public async Task<List<Carrito>> ObtenerCarritoPorUsuario(int usuarioId)
        {
            var carritos = await _carritoRepo.ListarPorUsuario(usuarioId);

            double totalOrden = 0;
            foreach (var c in carritos)
            {
                // Obtener el producto desde el repo
                var producto = await _productoRepo.ObtenerPorId(c.ProductoId);
                var precio = (double)(producto?.PrecioConDescuento ?? 0m);

                c.PrecioTotal = precio * c.Cantidad;
                totalOrden += c.PrecioTotal;
                c.TotalPrecioOrdenes = totalOrden;
            }

            return carritos;
        }


        public async Task<int> ContarCarrito(int usuarioId)
        {
            return await _carritoRepo.ContarPorUsuario(usuarioId);
        }
    }


}
