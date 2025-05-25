namespace EcomTrackAPI.Models
{
    public class ClienteActualizar
    {
        public required int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Correo { get; set; }
        public required string Pwd { get; set; }
        public required string Estado { get; set; }
        public required string Telefono { get; set; }
    }
}
