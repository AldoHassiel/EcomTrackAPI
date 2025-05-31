namespace EcomTrackAPI.Models
{
    public class DetalleCarrito
    {
        public int Id { get; set; }
        public int Carrito { get; set; }
        public int Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
    }
}
