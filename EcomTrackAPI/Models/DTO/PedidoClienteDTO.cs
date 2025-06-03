namespace EcomTrackAPI.Models.DTO
{
    public class PedidoClienteDTO
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
    }
}
