using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using tienda_electrodomesticos_api.Models;
using tienda_electrodomesticos_api.Repositorio.Interfaces;

namespace tienda_electrodomesticos_api.Repositorio.DAO
{
    public class ProductoDAO : IProductoRepository
    {
        private readonly string _connectionString;

        public ProductoDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Producto> Guardar(Producto producto)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_insertar_producto", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@titulo", producto.Titulo);
            cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
            cmd.Parameters.AddWithValue("@categoria_id", producto.CategoriaId);
            cmd.Parameters.AddWithValue("@precio", producto.Precio);
            cmd.Parameters.AddWithValue("@stock", producto.Stock);
            cmd.Parameters.AddWithValue("@imagen", producto.Imagen ?? string.Empty);
            cmd.Parameters.AddWithValue("@descuento", producto.Descuento);
            cmd.Parameters.AddWithValue("@is_active", producto.IsActive);

            await cmd.ExecuteNonQueryAsync();
            return producto;
        }

        public async Task Eliminar(Producto producto)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_eliminar_producto", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@producto_id", producto.Id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Producto?> ObtenerPorId(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_listar_productos", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if ((int)reader["id"] == id)
                {
                    return new Producto
                    {
                        Id = (int)reader["id"],
                        Titulo = (string)reader["titulo"],
                        Descripcion = (string)reader["descripcion"],
                        Precio = (decimal)reader["precio"],
                        Stock = (int)reader["stock"],
                        Descuento = (int)reader["descuento"],
                        PrecioConDescuento = (decimal)reader["precio_con_descuento"],
                        Imagen = reader["imagen"] as string,
                        IsActive = (bool)reader["is_active"],
                        CategoriaId = (int)reader["categoria_id"],
                        CategoriaNombre = (string)reader["categoria"]
                    };
                }
            }
            return null;
        }

        public async Task<List<Producto>> ListarTodos()
        {
            var lista = new List<Producto>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_listar_productos", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Producto
                {
                    Id = (int)reader["id"],
                    Titulo = (string)reader["titulo"],
                    Descripcion = (string)reader["descripcion"],
                    Precio = (decimal)reader["precio"],
                    Stock = (int)reader["stock"],
                    Descuento = (int)reader["descuento"],
                    PrecioConDescuento = (decimal)reader["precio_con_descuento"],
                    Imagen = reader["imagen"] as string,
                    IsActive = (bool)reader["is_active"],
                    CategoriaId = (int)reader["categoria_id"],
                    CategoriaNombre = (string)reader["categoria"]
                });
            }

            return lista;
        }

        public async Task<List<Producto>> ListarActivos()
        {
            var lista = new List<Producto>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_listar_productos_activos", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Producto
                {
                    Id = (int)reader["id"],
                    Titulo = (string)reader["titulo"],
                    Descripcion = (string)reader["descripcion"],
                    Precio = (decimal)reader["precio"],
                    Stock = (int)reader["stock"],
                    Descuento = (int)reader["descuento"],
                    PrecioConDescuento = (decimal)reader["precio_con_descuento"],
                    Imagen = reader["imagen"] as string,
                    IsActive = (bool)reader["is_active"],
                    CategoriaId = (int)reader["categoria_id"],
                    CategoriaNombre = (string)reader["categoria"]
                });
            }

            return lista;
        }


        public async Task<List<Producto>> ListarPorCategoria(string nombreCategoria)
        {
            var lista = new List<Producto>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("SELECT p.* FROM productos p " +
                                           "INNER JOIN categorias c ON p.categoria_id = c.id " +
                                           "WHERE c.nombre = @nombreCategoria", conn);
            cmd.Parameters.AddWithValue("@nombreCategoria", nombreCategoria);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Producto
                {
                    Id = (int)reader["id"],
                    Titulo = (string)reader["titulo"],
                    Descripcion = (string)reader["descripcion"],
                    Precio = (decimal)reader["precio"],
                    Stock = (int)reader["stock"],
                    Descuento = (int)reader["descuento"],
                    PrecioConDescuento = (decimal)reader["precio_con_descuento"],
                    Imagen = reader["imagen"] as string,
                    IsActive = (bool)reader["is_active"],
                    CategoriaId = (int)reader["categoria_id"],
                  
                });
            }

            return lista;
        }

        public async Task<List<Producto>> BuscarPorTituloOCategoria(string filtro)
        {
            filtro = filtro.ToLower();
            var lista = new List<Producto>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("SELECT p.* FROM productos p " +
                                           "INNER JOIN categorias c ON p.categoria_id = c.id " +
                                           "WHERE LOWER(p.titulo) LIKE @filtro OR LOWER(c.nombre) LIKE @filtro", conn);
            cmd.Parameters.AddWithValue("@filtro", filtro + "%");

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Producto
                {
                    Id = (int)reader["id"],
                    Titulo = (string)reader["titulo"],
                    Descripcion = (string)reader["descripcion"],
                    Precio = (decimal)reader["precio"],
                    Stock = (int)reader["stock"],
                    Descuento = (int)reader["descuento"],
                    PrecioConDescuento = (decimal)reader["precio_con_descuento"],
                    Imagen = reader["imagen"] as string,
                    IsActive = (bool)reader["is_active"],
                    CategoriaId = (int)reader["categoria_id"],
              
                });
            }

            return lista;
        }

        public Task Agregar(Producto producto) => Guardar(producto);

        public async Task Actualizar(Producto producto)
        {
            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_actualizar_producto", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", producto.Id);
            cmd.Parameters.AddWithValue("@titulo", producto.Titulo);
            cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
            cmd.Parameters.AddWithValue("@categoria_id", producto.CategoriaId);
            cmd.Parameters.AddWithValue("@precio", producto.Precio);
            cmd.Parameters.AddWithValue("@stock", producto.Stock);
            if (!string.IsNullOrEmpty(producto.Imagen))
            {
                cmd.Parameters.AddWithValue("@imagen", producto.Imagen);
            }
            else
            {
                cmd.Parameters.AddWithValue("@imagen", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@descuento", producto.Descuento);
            cmd.Parameters.AddWithValue("@is_active", producto.IsActive);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }


    }
}
