namespace EcomTrackAPI.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } 
        public int IdCategoria { get; set; }
        public string CategoriaNombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdProveedor { get; set; }
        public string ProveedorNombre { get; set; }
        public string UrlImagen { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }
    }
}