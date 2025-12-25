using Microsoft.AspNetCore.Mvc;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Models.DTOs;
using tienda_electrodomesticos_api.Service.Interfaces;

namespace tienda_electrodomesticos_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // POST: api/usuario/registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<Usuario>> RegistrarUsuario([FromBody] UsuarioRegistrarDto dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Rol = dto.Rol ?? "ROLE_USER", // si no envías rol, será usuario normal
                CuentaNoBloqueada = true,
                IntentoFallido = 0
            };
                
            var registrado = await _usuarioService.RegistrarUsuario(usuario, dto.Password);
            return Ok(registrado);
        }

        // GET: api/usuario/email?email=correo@dominio.com
        [HttpGet("email")]
        public async Task<ActionResult<Usuario>> ObtenerPorEmail([FromQuery] string email)
        {
            var usuario = await _usuarioService.ObtenerPorEmail(email);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // GET: api/usuario/rol?rol=ROLE_USER
        [HttpGet("rol")]
        public async Task<ActionResult<List<Usuario>>> ListarUsuariosPorRol([FromQuery] string rol)
        {
            var usuarios = await _usuarioService.ListarUsuariosPorRol(rol);
            return Ok(usuarios);
        }

        // PUT: api/usuario/estado/{id}
        [HttpPut("estado/{id}")]
        public async Task<ActionResult> ActualizarEstadoCuenta(int id, [FromBody] bool estado)
        {
            var actualizado = await _usuarioService.ActualizarEstadoCuenta(id, estado);
            if (!actualizado) return NotFound();
            return NoContent();
        }

        // PUT: api/usuario/reiniciar-contador/{id}
        [HttpPut("reiniciar-contador/{id}")]
        public async Task<ActionResult> ReiniciarContador(int id)
        {
            await _usuarioService.ReiniciarContador(id);
            return NoContent();
        }

        // PUT: api/usuario/actualizar
        [HttpPut("actualizar")]
        public async Task<ActionResult<Usuario>> ActualizarUsuario([FromBody] Usuario usuario)
        {
            var actualizado = await _usuarioService.ActualizarUsuario(usuario);
            return Ok(actualizado);
        }

        // PUT: api/usuario/reset-token
        [HttpPut("reset-token")]
        public async Task<ActionResult> ActualizarResetToken([FromBody] ResetTokenDto dto)
        {
            await _usuarioService.ActualizarResetToken(dto.Email, dto.ResetToken);
            return NoContent();
        }

        // GET: api/usuario/token?token=xyz
        [HttpGet("token")]
        public async Task<ActionResult<Usuario>> ObtenerPorToken([FromQuery] string token)
        {
            var usuario = await _usuarioService.ObtenerPorToken(token);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // POST: api/usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] UsuarioLoginDto dto)
        {
            var usuario = await _usuarioService.Login(dto.Email, dto.Password);

            if (usuario == null)
                return Unauthorized("Correo o contraseña incorrecta.");

            return Ok(usuario);
        }

    }
}
