namespace tienda_electrodomesticos_api.Models.DTOs
{
    public class UsuarioRegistrarDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
