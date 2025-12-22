using Microsoft.AspNetCore.Mvc;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }

    // DTO para el POST del carrito
    public class CarritoRequestDto
    {
        public int ProductoId { get; set; }
        public int UsuarioId { get; set; }
    }
}
