namespace EcomTrackAPI.Models.DTO
{
    public class CarritoDTO
    {
        public decimal Total { get; set; }
        public int CantidadProductos { get; set; }
        public bool EstaVacio { get; set; }
        public List<ProductoCarritoDTO> Productos { get; set; } = new List<ProductoCarritoDTO>();
    }
}
