using Microsoft.AspNetCore.Mvc;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using tienda_electrodomesticos_api.Models.DTOs;

namespace tienda_electrodomesticos_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        // GET: api/carrito/{usuarioId}
        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<List<Carrito>>> ObtenerCarritoPorUsuario(int usuarioId)
        {
            var carritos = await _carritoService.ObtenerCarritoPorUsuario(usuarioId);
            return Ok(carritos);
        }

        // POST: api/carrito
        [HttpPost]
        public async Task<ActionResult<Carrito>> GuardarCarrito([FromBody] CarritoRequestDto dto)
        {
            var resultado = await _carritoService.GuardarCarrito(dto.ProductoId, dto.UsuarioId);
            return Ok(resultado);
        }

        // GET: api/carrito/contar/{usuarioId}
        [HttpGet("contar/{usuarioId}")]
        public async Task<ActionResult<int>> ContarCarrito(int usuarioId)
        {
            var cantidad = await _carritoService.ContarCarrito(usuarioId);
            return Ok(cantidad);
        }

        // DELETE: api/carrito/{usuarioId}/{productoId}
        [HttpDelete("{usuarioId}/{productoId}")]
        public async Task<ActionResult> EliminarProducto(int usuarioId, int productoId)
        {
            var eliminado = await _carritoService.EliminarProducto(usuarioId, productoId);
            if (eliminado)
                return Ok(new { mensaje = "Producto eliminado del carrito." });
            else
                return NotFound(new { mensaje = "No se encontró el producto en el carrito." });
        }

        // PUT api/carrito/aumentar/{usuarioId}/{productoId}
        [HttpPut("aumentar/{usuarioId}/{productoId}")]
        public async Task<ActionResult> AumentarCantidad(int usuarioId, int productoId)
        {
            var exito = await _carritoService.AumentarCantidad(usuarioId, productoId);
            return exito ? Ok() : NotFound();
        }

        // PUT api/carrito/disminuir/{usuarioId}/{productoId}
        [HttpPut("disminuir/{usuarioId}/{productoId}")]
        public async Task<ActionResult> DisminuirCantidad(int usuarioId, int productoId)
        {
            var exito = await _carritoService.DisminuirCantidad(usuarioId, productoId);
            return exito ? Ok() : NotFound();
        }

    }
}
