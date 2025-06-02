using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class EstadoMexicoService
    {
        public static RespuestaAPI ObtenerEstados()
        {
           var res = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            string query = "SELECT Id, Nombre FROM Estados";
            SqlCommand cmd = new SqlCommand(query, conx);

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<EstadosMexico> estados = new List<EstadosMexico>();
                while (reader.Read())
                {
                    var estado = new EstadosMexico
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre"))
                    };
                    estados.Add(estado);
                }

                res.Resultado = true;
                res.Mensaje = "Estados obtenidos correctamente.";
                res.Datos = estados;
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = $"Error inesperado: {ex.Message}";
                res.Datos = null;
            }
            finally
            {
                conx.Close();
            }
            return res;
        }
    }
}
