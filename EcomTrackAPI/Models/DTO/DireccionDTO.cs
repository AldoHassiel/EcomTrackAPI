namespace EcomTrackAPI.Models.DTO
{
    public class DireccionDTO
    {
        public int Id { get; set; }
        public int TipoDireccion { get; set; }
        public string TipoDireccionNombre { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string EntreCalles { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }
        public bool EsPredeterminado { get; set; }
    }
}
