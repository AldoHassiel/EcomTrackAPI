namespace EcomTrackAPI.Models
{
    public class Envio
    {
        public int Id { get; set; }
        public DateTime? FechaEnvio { get; set; } // Cambiado a DateTime?
        public DateTime? FechaEntrega { get; set; } // Cambiado a DateTime?
        public int IdPedido { get; set; }
        public int? IdTransportista { get; set; } // Cambiado a int?
        public string NombreTransportista { get; set; }
        public int? IdEstado { get; set; } // Cambiado a int?
        public string DescripcionEstado { get; set; }
    }
}
