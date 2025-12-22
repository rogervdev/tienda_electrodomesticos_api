using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Repositorio.Interfaces;
using tienda_electrodomesticos_api.Service.Interfaces;
using BCrypt.Net;

namespace tienda_electrodomesticos_api.Service.Impl
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> RegistrarUsuario(Usuario usuario, string password)
        {
            usuario.Rol = "ROLE_USER";
            usuario.CuentaNoBloqueada = true;
            usuario.IntentoFallido = 0;

            await _usuarioRepository.Agregar(usuario, password);
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

            usuario.CuentaNoBloqueada = estado;
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
    }
}
