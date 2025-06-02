using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class TipoDireccionService
    {
        public static RespuestaAPI ObtenerTipoDireccion()
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();

            string query = "select Id, Descripcion FROM TipoDireccion";
            SqlCommand cmd = new SqlCommand(query, conx);

            List<TipoDireccion> tiposDireccion = new List<TipoDireccion>();

            try
            {
                conx.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var tipo = new TipoDireccion() {
                        Id = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                    };
                    tiposDireccion.Add(tipo);
                }

                if(tiposDireccion.Count == 0)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No hay tipos";
                    respuestaAPI.Datos = null;
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Cargados";
                respuestaAPI.Datos = tiposDireccion;

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

