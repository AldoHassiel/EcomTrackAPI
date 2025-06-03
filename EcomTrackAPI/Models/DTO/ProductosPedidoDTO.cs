namespace EcomTrackAPI.Models.DTO
{
    public class ProductosPedidoDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal SubtotalProducto { get; set; }
        public string UrlImagen { get; set; }
    }
}
