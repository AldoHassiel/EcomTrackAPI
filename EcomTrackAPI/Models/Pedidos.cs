namespace EcomTrackAPI.Models
{
    public class Pedidos
    {
        public int Id { get; set; }
        public int Cliente { get; set; }
        public decimal Importe {  get; set; }
        public int Estado { get; set; }


    }
}
