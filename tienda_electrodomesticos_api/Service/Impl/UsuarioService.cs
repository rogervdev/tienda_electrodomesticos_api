using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Models.DTOs;
using tienda_electrodomesticos_api.Repositorio.Interfaces;
using tienda_electrodomesticos_api.Service.Interfaces;

namespace tienda_electrodomesticos_api.Service.Impl
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> RegistrarUsuario(UsuarioRegistrarDto dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Rol = string.IsNullOrEmpty(dto.Rol) ? "ROLE_USER" : dto.Rol, // <- aquí
                CuentaNoBloqueada = true,
                IntentoFallido = 0
            };

            // Asignar la contraseña usando tu repositorio
            await _usuarioRepository.Agregar(usuario, dto.Password);

            return usuario;
        }


        public async Task<Usuario?> ObtenerPorEmail(string email)
        {
            return await _usuarioRepository.BuscarPorEmail(email);
        }

        public async Task<bool> ExistePorEmail(string email)
        {
            return await _usuarioRepository.ExistePorEmail(email);
        }

        public async Task<List<Usuario>> ListarUsuariosPorRol(string rol)
        {
            return await _usuarioRepository.ListarPorRol(rol);
        }

        public async Task<bool> ActualizarEstadoCuenta(int id, bool estado)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(id);
            if (usuario == null) return false;

            // ✅ Activar o desactivar usuario desde panel de administración
            usuario.IsEnable = estado;

            await _usuarioRepository.Actualizar(usuario);
            return true;
        }


        public async Task IncrementarIntentoFallido(Usuario usuario)
        {
            usuario.IntentoFallido++;
            await _usuarioRepository.Actualizar(usuario);
        }

        public async Task BloquearCuenta(Usuario usuario)
        {
            usuario.CuentaNoBloqueada = false;
            usuario.LockTime = DateTime.UtcNow;
            await _usuarioRepository.Actualizar(usuario);
        }

        public async Task<bool> DesbloquearCuentaPorTiempo(Usuario usuario, long unlockDurationMilliseconds)
        {
            if (usuario.LockTime == null) return false;

            var unlockTime = usuario.LockTime.Value.AddMilliseconds(unlockDurationMilliseconds);
            if (DateTime.UtcNow >= unlockTime)
            {
                usuario.CuentaNoBloqueada = true;
                usuario.IntentoFallido = 0;
                usuario.LockTime = null;
                await _usuarioRepository.Actualizar(usuario);
                return true;
            }

            return false;
        }

        public async Task ReiniciarContador(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObtenerPorId(usuarioId);
            if (usuario == null) return;

            usuario.IntentoFallido = 0;
            await _usuarioRepository.Actualizar(usuario);
        }

        public async Task ActualizarResetToken(string email, string resetToken)
        {
            var usuario = await _usuarioRepository.BuscarPorEmail(email);
            if (usuario == null) return;

            usuario.ResetToken = resetToken;
            await _usuarioRepository.Actualizar(usuario);
        }

        public async Task<Usuario?> ObtenerPorToken(string token)
        {
            return await _usuarioRepository.BuscarPorResetToken(token);
        }

        public async Task<Usuario> ActualizarUsuario(Usuario usuario)
        {
            await _usuarioRepository.Actualizar(usuario);
            return usuario;
        }

        public async Task<Usuario?> Login(string email, string password)
        {
            var usuario = await _usuarioRepository.BuscarPorEmail(email);
            if (usuario == null)
                return null;

            // 1️⃣ Deshabilitado por el admin
            if (!usuario.IsEnable)
                return null;

            // 2️⃣ Bloqueo por intentos fallidos
            if (!usuario.CuentaNoBloqueada && usuario.LockTime != null)
            {
                // ⏱️ desbloqueo automático después de 20 segundos
                if (DateTime.UtcNow >= usuario.LockTime.Value.AddSeconds(20))
                {
                    usuario.CuentaNoBloqueada = true;
                    usuario.IntentoFallido = 0;
                    usuario.LockTime = null;
                    await _usuarioRepository.Actualizar(usuario);
                }
                else
                {
                    return null;
                }
            }

            // 3️⃣ Validar contraseña
            bool passwordCorrecta = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
            if (!passwordCorrecta)
            {
                usuario.IntentoFallido++;

                if (usuario.IntentoFallido >= 3)
                {
                    usuario.CuentaNoBloqueada = false;
                    usuario.LockTime = DateTime.UtcNow;
                }

                await _usuarioRepository.Actualizar(usuario);
                return null;
            }

            // 4️⃣ Login correcto → reset
            usuario.IntentoFallido = 0;
            usuario.LockTime = null;
            await _usuarioRepository.Actualizar(usuario);

            return usuario;
        }



    }
}
