using Microsoft.Data.SqlClient;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Repositorio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tienda_electrodomesticos_api.Repositorio.DAO
{
    public class CategoriaDAO : ICategoriaRepository
    {
        private readonly string _connectionString;

        public CategoriaDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Agregar(Categoria categoria)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_insertar_categoria", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nombre", categoria.Nombre);
            cmd.Parameters.AddWithValue("@imagen_nombre", categoria.ImagenNombre ?? string.Empty);
            cmd.Parameters.AddWithValue("@is_active", categoria.IsActive);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_actualizar_categoria", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", categoria.Id);
            cmd.Parameters.AddWithValue("@nombre", categoria.Nombre);
            cmd.Parameters.AddWithValue("@imagen_nombre", categoria.ImagenNombre ?? string.Empty);
            cmd.Parameters.AddWithValue("@is_active", categoria.IsActive);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Eliminar(Categoria categoria)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_eliminar_categoria", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@categoria_id", categoria.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> ExistePorNombre(string nombre)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_existe_categoria_por_nombre", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nombre", nombre);

            var result = await cmd.ExecuteScalarAsync();
            return (result != null && (int)result > 0);
        }

        public async Task<Categoria?> ObtenerPorId(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_obtener_categoria_por_id", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Categoria
                {
                    Id = (int)reader["id"],
                    Nombre = (string)reader["nombre"],
                    ImagenNombre = reader["imagen_nombre"] as string,
                    IsActive = (bool)reader["is_active"]
                };
            }
            return null;
        }

        public async Task<List<Categoria>> ListarTodas()
        {
            var lista = new List<Categoria>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_listar_categorias_combo", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Categoria
                {
                    Id = (int)reader["id"],
                    Nombre = (string)reader["nombre"],
                    ImagenNombre = reader["imagen_nombre"] as string, // <--- ahora sí se asigna
                    IsActive = true
                });
            }
            return lista;
        }

        public async Task<List<Categoria>> ListarTodasCategorias()
        {
            var lista = new List<Categoria>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_listar_categorias_todas", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Categoria
                {
                    Id = (int)reader["id"],
                    Nombre = (string)reader["nombre"],
                    ImagenNombre = reader["imagen_nombre"] as string,
                    IsActive = (bool)reader["is_active"] // ahora toma el valor real
                });
            }
            return lista;
        }


        public async Task<List<Categoria>> ListarActivas()
        {
            var lista = new List<Categoria>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_listar_categorias_combo", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Categoria
                {
                    Id = (int)reader["id"],
                    Nombre = (string)reader["nombre"],
                    ImagenNombre = reader["imagen_nombre"] as string, // <--- ahora sí se asigna
                    IsActive = true
                });
            }
            return lista;
        }

    }
}
