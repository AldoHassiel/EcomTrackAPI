using EcomTrackAPI.Models;
using EcomTrackAPI.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class DireccionesService
    {
        public static RespuestaAPI ObtenerDirecciones(int idCliente)
        {
            var respuesta = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerDirecciones", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IdCliente", idCliente);

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
                        IdEstado = reader.GetInt32(reader.GetOrdinal("IdEstado")),
                        EstadoNombre = reader.GetString(reader.GetOrdinal("EstadoNombre")),
                        CodigoPostal = reader.GetString(reader.GetOrdinal("CodigoPostal")),
                        EsPredeterminado = reader.GetBoolean(reader.GetOrdinal("EsPredeterminado")),
                        EstaActiva = reader.GetBoolean(reader.GetOrdinal("EstaActiva"))
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
        public static RespuestaAPI AgregarDireccion(DireccionAgregarDTO direccion)
        {
            var respuesta = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("AgregarDireccion", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IdCliente", direccion.IdCliente);
            cmd.Parameters.AddWithValue("@TipoDireccion", direccion.TipoDireccion);
            cmd.Parameters.AddWithValue("@Calle", direccion.Calle);
            cmd.Parameters.AddWithValue("@Numero", direccion.Numero);
            cmd.Parameters.AddWithValue("@EntreCalles", direccion.EntreCalles);
            cmd.Parameters.AddWithValue("@Colonia", direccion.Colonia);
            cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
            cmd.Parameters.AddWithValue("@IdEstado", direccion.IdEstado);
            cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                respuesta.Resultado = true;
                respuesta.Mensaje = "Dirección agregada correctamente.";
                respuesta.Datos = null;
            }
            catch (Exception ex)
            {
                respuesta.Resultado = false;
                respuesta.Mensaje = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                conx.Close();
            }

            return respuesta;
        }
        public static RespuestaAPI EditarDireccion(DireccionEditarDTO direccion)
        {
            var respuesta = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EditarDireccion", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", direccion.Id);
            cmd.Parameters.AddWithValue("@IdCliente", direccion.IdCliente);
            cmd.Parameters.AddWithValue("@TipoDireccion", direccion.TipoDireccion);
            cmd.Parameters.AddWithValue("@Calle", direccion.Calle);
            cmd.Parameters.AddWithValue("@Numero", direccion.Numero);
            cmd.Parameters.AddWithValue("@EntreCalles", direccion.EntreCalles);
            cmd.Parameters.AddWithValue("@Colonia", direccion.Colonia);
            cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
            cmd.Parameters.AddWithValue("@IdEstado", direccion.IdEstado);
            cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);
            cmd.Parameters.AddWithValue("@EsPredeterminado", direccion.EsPredeterminado);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                respuesta.Resultado = true;
                respuesta.Mensaje = "Dirección editada correctamente.";
            }
            catch (Exception ex)
            {
                respuesta.Resultado = false;
                respuesta.Mensaje = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                conx.Close();
            }

            return respuesta;
        }
        public static RespuestaAPI EliminarDireccion(int id, int idCliente)
        {
            var respuesta = new RespuestaAPI();
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarDireccion", conx);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@IdCliente", idCliente);
            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();
                respuesta.Resultado = true;
                respuesta.Mensaje = "Dirección eliminada correctamente.";
            }
            catch (Exception ex)
            {
                respuesta.Resultado = false;
                respuesta.Mensaje = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                conx.Close();
            }
            return respuesta;
        }
    }
}
