namespace EcomTrackAPI.Models
{
    public class Envio
    {
        public int Id { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int IdPedido { get; set; }
        public int IdTransportista { get; set; }
        public int IdEstado { get; set; }

    }
}
