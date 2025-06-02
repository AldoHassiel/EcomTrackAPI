using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace EcomTrackAPI.Services
{
    public class EstadoService
    {
        public static RespuestaAPI ObtenerEstado()
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();

            string query = "select Id, Descripcion FROM Estado";
            SqlCommand cmd = new SqlCommand(query, conx);

            List<Estado> estados = new List<Estado>();

            try
            {
                conx.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var estado = new Estado()
                    {
                        Id = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                    };
                    estados.Add(estado);
                }

                if (estados.Count == 0)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No hay estados";
                    respuestaAPI.Datos = null;
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Cargados";
                respuestaAPI.Datos = estados;

            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = ex.Message;
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
    }
}
