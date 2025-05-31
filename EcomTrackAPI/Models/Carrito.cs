namespace EcomTrackAPI.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public int Cliente { get; set; }
        public bool Activo { get; set; }
        public List<DetalleCarrito> Productos { get; set; }
    }
}
