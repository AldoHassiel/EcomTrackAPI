namespace EcomTrackAPI.Models
{
    public class Pedidos
    {
        public int Id { get; set; }
        public int Cliente { get; set; }
        public string NombreCliente { get; set; }
        public decimal Importe { get; set; }
        public DateTime FechaPedido { get; set; }
        public int Estado { get; set; }
        public int Direccion { get; set; }

    }
}
