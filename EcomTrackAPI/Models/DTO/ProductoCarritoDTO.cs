namespace EcomTrackAPI.Models.DTO
{
    public class ProductoCarritoDTO
    {
        public int Id { get; set; }
        public int Producto { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public string UrlImagen { get; set; }
    }
}
