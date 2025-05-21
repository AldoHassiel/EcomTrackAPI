namespace EcomTrackAPI.Models
{
    public class Cliente
    {
        public required int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Correo { get; set; }
        public string? Pwd { get; set; }
        public required string Telefono { get; set; }
        public required string Estado { get; set; }
        public required int TotalCompras { get; set; }
        public required decimal TotalGastado { get; set; }
        public required DateTime FechaRegistro { get; set; }
    }
}
