using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Repositorio.Interfaces;

namespace tienda_electrodomesticos_api.Repositorio.DAO
{
    public class CarritoDAO : ICarritoRepository
    {
        private readonly string _connectionString;

        public CarritoDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Eliminar(Carrito carrito)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_eliminar_carrito", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@usuario_id", carrito.UsuarioId);
            cmd.Parameters.AddWithValue("@producto_id", carrito.ProductoId);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<Carrito?> BuscarPorProductoYUsuario(int productoId, int usuarioId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_buscar_carrito_producto_usuario", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@producto_id", productoId);
            cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Carrito
                {
                    Id = (int)reader["id"],
                    ProductoId = (int)reader["producto_id"],
                    UsuarioId = (int)reader["usuario_id"],
                    Cantidad = (int)reader["cantidad"]
                };
            }

            return null;
        }


        public async Task<int> ContarPorUsuario(int usuarioId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_contar_carrito_usuario", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

            return (int)await cmd.ExecuteScalarAsync();
        }


        public async Task<List<Carrito>> ListarPorUsuario(int usuarioId)
        {
            var lista = new List<Carrito>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_listar_carrito_usuario", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Carrito
                {
                    Id = (int)reader["id"],
                    ProductoId = (int)reader["producto_id"],
                    UsuarioId = (int)reader["usuario_id"],
                    Cantidad = (int)reader["cantidad"]
                });
            }

            return lista;
        }


        public async Task Guardar(Carrito carrito)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd;
            if (carrito.Id == 0)
            {
                // Nuevo registro
                cmd = new SqlCommand("sp_insertar_carrito", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuario_id", carrito.UsuarioId);
                cmd.Parameters.AddWithValue("@producto_id", carrito.ProductoId);
                cmd.Parameters.AddWithValue("@cantidad", carrito.Cantidad);
            }
            else
            {
                // Actualizar registro existente
                cmd = new SqlCommand("sp_actualizar_carrito", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", carrito.Id);
                cmd.Parameters.AddWithValue("@cantidad", carrito.Cantidad);
            }

            await cmd.ExecuteNonQueryAsync();
        }



    }
}
