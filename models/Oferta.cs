namespace JobConnectAPI.Models
{
    public class Oferta
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string? Salario { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaPublicacion { get; set; }
    }
}