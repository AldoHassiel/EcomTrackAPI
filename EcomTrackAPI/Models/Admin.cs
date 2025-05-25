namespace EcomTrackAPI.Models
{
    public class Admin
    {
        public required int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Correo { get; set; }
        public string? Pwd { get; set; }
        public required string Telefono { get; set; }
        public required string Estado { get; set; } = "Activo";
        public required DateTime FechaRegistro { get; set; }
    }
}
