namespace EcomTrackAPI.Models.DTO
{
    public class TarjetaAgregarDTO
    {
        public int Cliente { get; set; }
        public string NumeroTarjeta { get; set; }
        public int MetodoDePago { get; set; }
        public string MarcaDeTarejta { get; set; }

    }
}
