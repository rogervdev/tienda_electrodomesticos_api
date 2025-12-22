using Microsoft.Data.SqlClient;
using tienda_electrodomesticos_api.Models;
using BCrypt.Net;

namespace tienda_electrodomesticos_api.Config
{
    public static class DataInitializer
    {
        public static async Task InicializarAdmin(string connectionString)
        {
            string emailAdmin = "admin@correo.com";
            string passwordAdmin = "123456";

            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            // Verificar si ya existe
            string sqlCheck = "SELECT COUNT(1) FROM usuarios WHERE email = @Email";
            using SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn);
            cmdCheck.Parameters.AddWithValue("@Email", emailAdmin);
            int count = (int)await cmdCheck.ExecuteScalarAsync();

            if (count == 0)
            {
                // Insertar admin
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordAdmin);

                string sqlInsert = @"
                    INSERT INTO usuarios (nombre, apellido, email, passwordhash, rol, profile_image, is_enable)
                    VALUES (@Nombre, @Apellido, @Email, @PasswordHash, @Rol, @ProfileImage, 1)";

                using SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.AddWithValue("@Nombre", "Admin");
                cmdInsert.Parameters.AddWithValue("@Apellido", "Principal");
                cmdInsert.Parameters.AddWithValue("@Email", emailAdmin);
                cmdInsert.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmdInsert.Parameters.AddWithValue("@Rol", "admin");
                cmdInsert.Parameters.AddWithValue("@ProfileImage", "default.png");

                await cmdInsert.ExecuteNonQueryAsync();
            }
        }
    }
}
