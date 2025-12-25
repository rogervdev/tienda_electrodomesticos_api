using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Models.DTOs;
using tienda_electrodomesticos_api.Service.Interfaces;

namespace tienda_electrodomesticos_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: api/categoria
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetAllCategorias()
        {
            var categorias = await _categoriaService.GetAllCategorias();
            return Ok(categorias);
        }

        // GET: api/categoria/activos
        [HttpGet("activos")]
        public async Task<ActionResult<List<Categoria>>> GetAllActiveCategorias()
        {
            var categorias = await _categoriaService.GetAllActiveCategorias();
            return Ok(categorias);
        }

        // GET: api/categoria/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoriaById(int id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorId(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        // POST: api/categoria
        [HttpPost]
        public async Task<ActionResult<Categoria>> GuardarCategoria([FromForm] CategoriaUploadDto dto)
        {
            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                IsActive = dto.IsActive
            };

            var cat = await _categoriaService.GuardarCategoria(categoria, dto.Imagen);
            return Ok(cat);
        }


        // PUT: api/categoria/{id}
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Categoria>> ActualizarCategoria(
            int id,
            [FromForm] CategoriaUploadDto dto)
        {
            var catDB = await _categoriaService.ObtenerCategoriaPorId(id);
            if (catDB == null) return NotFound();

            catDB.Nombre = dto.Nombre;
            catDB.IsActive = dto.IsActive;

            await _categoriaService.ActualizarCategoria(catDB, dto.Imagen);
            return Ok(catDB);
        }


        // DELETE: api/categoria/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarCategoria(int id)
        {
            var result = await _categoriaService.EliminarCategoria(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
