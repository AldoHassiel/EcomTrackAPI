using EcomTrackAPI.Models.DTO;
namespace EcomTrackAPI.Models
{
    public class PedidoDetalleClienteDTO
    {
        public List<ProductosPedidoDTO> Productos { get; set; } = new List<ProductosPedidoDTO>();

        public string NumeroTarjeta { get; set; }
        public string MetodoPago { get; set; }
        public string MarcaTarjeta { get; set; }

        public string NombreTipoDireccion { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string EntreCalles { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string EstadoDireccion { get; set; }
        public string CodigoPostal { get; set; }

        public string EstadoPedido { get; set; }
        public decimal Importe { get; set; }
        public string Transportista { get; set; }
    }
}
