namespace EcomTrackAPI.Models
{
    public class Pedidos
    {
        public int Id { get; set; }
        public int Cliente { get; set; }
        public string NombreCliente { get; set; }
        public decimal Importe { get; set; }
        public DateTime FechaPedido { get; set; }
        public int Estado { get; set; }
        public string Calle { get; set; }
        public string EntreCalles { get; set; }
        public string Numero { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string EstadoD { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }


    }
}
