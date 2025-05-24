using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EcomTrackAPI.Models
{
    public class Direccion
    {
        public int Id { get; set; }
        public int Cliente { get; set; }
	    public int Tipo { get; set; }
        public string Calle { get; set; }
        public string EntreCalles { get; set; }
        public string Numero { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }
    }
}
