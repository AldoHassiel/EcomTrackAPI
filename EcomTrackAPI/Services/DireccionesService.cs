using EcomTrackAPI.Models;
using EcomTrackAPI.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class DireccionesService
    {
        public static RespuestaAPI ObtenerDirecciones(int cliente)
        {
            var respuesta = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerDirecciones", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Cliente", cliente);

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<DireccionDTO> direcciones = new List<DireccionDTO>();

                while (reader.Read())
                {
                    var direccion = new DireccionDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        TipoDireccion = reader.GetInt32(reader.GetOrdinal("TipoDireccion")),
                        TipoDireccionNombre = reader.GetString(reader.GetOrdinal("TipoDireccionNombre")),
                        Calle = reader.GetString(reader.GetOrdinal("Calle")),
                        Numero = reader.GetString(reader.GetOrdinal("Numero")),
                        EntreCalles = reader.GetString(reader.GetOrdinal("EntreCalles")),
                        Colonia = reader.GetString(reader.GetOrdinal("Colonia")),
                        Ciudad = reader.GetString(reader.GetOrdinal("Ciudad")),
                        Estado = reader.GetString(reader.GetOrdinal("Estado")),
                        CodigoPostal = reader.GetString(reader.GetOrdinal("CodigoPostal")),
                        Pais = reader.GetString(reader.GetOrdinal("Pais")),
                        EsPredeterminado = reader.GetBoolean(reader.GetOrdinal("EsPredeterminado"))
                    };
                    direcciones.Add(direccion);
                }

                if (direcciones.Count == 0)
                {
                    respuesta.Resultado = false;
                    respuesta.Mensaje = "No se encontraron direcciones para el cliente especificado.";
                    respuesta.Datos = null;
                }
                else
                {
                    respuesta.Resultado = true;
                    respuesta.Mensaje = "Direcciones obtenidas correctamente.";
                    respuesta.Datos = direcciones;
                }
            }
            catch (Exception ex)
            {
                respuesta.Resultado = false;
                respuesta.Mensaje = $"Error inesperado: {ex.Message}";
                respuesta.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuesta;
        }
    }
}
