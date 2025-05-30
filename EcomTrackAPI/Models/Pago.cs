namespace EcomTrackAPI.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int Pedido { get; set; }
        public int MetodoPago { get; set; }
        public decimal ImporteAbonado { get; set; }
        public string Tarjeta { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
