namespace EcomTrackAPI.Models.DTO
{
    public class PedidoPeriodoList
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public string MarcaTarjeta { get; set; }
        public string Estado { get; set; }
    }
}
