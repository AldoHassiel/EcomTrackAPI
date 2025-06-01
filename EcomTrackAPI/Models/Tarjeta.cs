namespace EcomTrackAPI.Models
{
    public class Tarjeta
    {
        public int Id { get; set; }
        public int Cliente { get; set; }
        public string NumeroTarjeta { get; set; }
        //id del tipo
        public int MetodoDePago { get; set; }
        //debito o credito
        public string TipoTarjeta {get; set;}
        public string MarcaTarjeta { get; set; }
        public bool Activa { get; set; }
        public bool Predeterminada { get; set; }
        public DateTime FechaVencimiento { get; set; }

    }
}
