namespace EcomTrackAPI.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int IdPedido{ get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}