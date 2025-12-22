using BCrypt.Net;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Repositorio.Interfaces;

namespace tienda_electrodomesticos_api.Repositorio.DAO
{
    public class UsuarioDAO : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Usuario?> BuscarPorEmail(string email)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_buscar_usuario_por_email", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    Id = (int)reader["id"],
                    Nombre = (string)reader["nombre"],
                    Apellido = (string)reader["apellido"],
                    Email = (string)reader["email"],
                    PasswordHash = (string)reader["passwordhash"],
                    ProfileImage = reader["profile_image"] as string ?? "",
                    Rol = (string)reader["rol"],
                    IsEnable = (bool)reader["is_enable"]
                };
            }
            return null;
        }

        public async Task<bool> ExistePorEmail(string email)
        {
            var usuario = await BuscarPorEmail(email);
            return usuario != null;
        }

        public async Task<List<Usuario>> ListarPorRol(string rol)
        {
            var lista = new List<Usuario>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_listar_usuarios_por_rol", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@rol", rol);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Usuario
                {
                    Id = (int)reader["id"],
                    Nombre = (string)reader["nombre"],
                    Apellido = (string)reader["apellido"],
                    Email = (string)reader["email"],
                    PasswordHash = (string)reader["passwordhash"],
                    ProfileImage = reader["profile_image"] as string ?? "",
                    Rol = (string)reader["rol"],
                    IsEnable = (bool)reader["is_enable"]
                });
            }
            return lista;
        }

        public async Task<Usuario?> BuscarPorResetToken(string token)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_buscar_usuario_por_reset_token", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@token", token);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    Id = (int)reader["id"],
                    Nombre = (string)reader["nombre"],
                    Apellido = (string)reader["apellido"],
                    Email = (string)reader["email"],
                    PasswordHash = (string)reader["passwordhash"],
                    ProfileImage = reader["profile_image"] as string ?? "",
                    Rol = (string)reader["rol"],
                    IsEnable = (bool)reader["is_enable"]
                };
            }
            return null;
        }

        public async Task<Usuario?> ObtenerPorId(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_obtener_usuario_por_id", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    Id = (int)reader["id"],
                    Nombre = (string)reader["nombre"],
                    Apellido = (string)reader["apellido"],
                    Email = (string)reader["email"],
                    PasswordHash = (string)reader["passwordhash"],
                    ProfileImage = reader["profile_image"] as string ?? "",
                    Rol = (string)reader["rol"],
                    IsEnable = (bool)reader["is_enable"]
                };
            }
            return null;
        }

        public async Task Agregar(Usuario usuario, string password)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_insertar_usuario", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@email", usuario.Email);
            cmd.Parameters.AddWithValue("@passwordhash", BCrypt.Net.BCrypt.HashPassword(password));
            cmd.Parameters.AddWithValue("@rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@profile_image", usuario.ProfileImage);
            cmd.Parameters.AddWithValue("@is_enable", usuario.IsEnable);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Actualizar(Usuario usuario)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_actualizar_usuario", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", usuario.Id);
            cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@email", usuario.Email);
            cmd.Parameters.AddWithValue("@rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@profile_image", usuario.ProfileImage);
            cmd.Parameters.AddWithValue("@is_enable", usuario.IsEnable);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Eliminar(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_eliminar_usuario", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
