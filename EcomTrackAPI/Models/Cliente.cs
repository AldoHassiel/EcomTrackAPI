namespace EcomTrackAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Pwd { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}