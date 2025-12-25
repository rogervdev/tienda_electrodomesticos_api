using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Models.DTOs;
using tienda_electrodomesticos_api.Service.Interfaces;

namespace tienda_electrodomesticos_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: api/producto
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetAllProductos()
        {
            var productos = await _productoService.GetAllProductos();
            return Ok(productos);
        }


        // GET: api/producto/activos
        [HttpGet("activos")]
        public async Task<ActionResult<List<Producto>>> GetAllActiveProductos([FromQuery] string? categoria)
        {
            var productos = await _productoService.GetAllActiveProductos(categoria ?? "");
            return Ok(productos);
        }


        // GET: api/producto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProductoById(int id)
        {
            var producto = await _productoService.GetProductoById(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }


        // POST: api/producto
        [HttpPost]
        public async Task<ActionResult<Producto>> GuardarProducto([FromForm] ProductoUploadDto dto)
        {
            var producto = new Producto
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                CategoriaId = dto.CategoriaId,
                Precio = dto.Precio,
                Stock = dto.Stock,
                Descuento = dto.Descuento,
                IsActive = dto.IsActive
                // NO seteamos PrecioConDescuento aquí
            };

            var prod = await _productoService.GuardarProducto(producto, dto.Imagen);
            return Ok(prod);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ActualizarProducto([FromForm] ProductoUpdateDto dto)
        {
            var producto = new Producto
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                CategoriaId = dto.CategoriaId,
                Precio = dto.Precio,
                Stock = dto.Stock,
                Descuento = dto.Descuento,
                IsActive = dto.IsActive,

                // 👇 conservar imagen actual
                Imagen = dto.Imagen
            };

            await _productoService.ActualizarProducto(producto, dto.ImagenFile);

            return Ok(producto);
        }





        // DELETE: api/producto/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarProducto(int id)
        {
            var result = await _productoService.EliminarProducto(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // GET: api/producto/buscar?query=tele
        [HttpGet("buscar")]
        public async Task<ActionResult<List<Producto>>> BuscarProducto([FromQuery] string query)
        {
            var productos = await _productoService.BuscarProducto(query);
            return Ok(productos);
        }

    }
}
