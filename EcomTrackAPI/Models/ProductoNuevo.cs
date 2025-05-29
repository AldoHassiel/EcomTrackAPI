namespace EcomTrackAPI.Models
{
    public class ProductoNuevo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdProveedor { get; set; }
        public string UrlImagen { get; set; }
        public string Estado { get; set; }
    }
}
