namespace JobConnectAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Rol { get; set; } = "usuario";
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}