namespace EcomTrackAPI.Models
{
    public class Sesion
    {
        public bool Autenticado { get; set; }
        public string? Mensaje { get; set; }
        public int IdUsuario { get; set; }
        public bool EsAdmin { get; set; }
    }
}
