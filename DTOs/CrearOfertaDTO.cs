namespace JobConnectAPI.DTOs
{
    public class CrearOfertaDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string? Salario { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}