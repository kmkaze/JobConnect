namespace JobConnectAPI.Models
{
    public class Postulacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int OfertaId { get; set; }
        public DateTime FechaPostulacion { get; set; }
    }
}