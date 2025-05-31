namespace EcomTrackAPI.Models
{
    public class Tarjeta
    {
        public int Id { get; set; }
        public int Cliente { get; set; }
        public string NumeroTarjeta { get; set; }
        public int MetodoDePago { get; set; }
        public string TipoTarjeta {get; set;}
        public bool Activa { get; set; }
    }
}
