using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class TarjetaService
    {
        public static RespuestaAPI ObtenerTarjetas(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            List<Tarjeta> tarjetas = new List<Tarjeta>();

            SqlConnection conx = Datos.conectar();
            string query = "SELECT t.NumeroTarjeta, mp.Descripcion AS TipoTarjeta " +
                "FROM Tarjeta t INNER JOIN MetodoPago mp ON " +
                "t.MetodoDePago = mp.Id WHERE t.Cliente = "+id+" AND t.Activa = 1";

            SqlCommand cmd = new SqlCommand(query, conx);

            try
            {
                conx.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                { 
                    Tarjeta tarjeta = new Tarjeta()
                    {
                        NumeroTarjeta = reader.GetString(0),
                        TipoTarjeta = reader.GetString(1)
                    };
                    tarjetas.Add(tarjeta);
                }

                if (tarjetas.Count == 0)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No se encontraron Tarjetas";
                    respuestaAPI.Datos = null;
                    return respuestaAPI;
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Tarjetas obtenidas correctamente";
                respuestaAPI.Datos = tarjetas;

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
        public static RespuestaAPI GuardarTarjeta(Tarjeta tarjeta)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarTarjeta", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cliente", tarjeta.Cliente);
            cmd.Parameters.AddWithValue("@NumeroTarjeta", tarjeta.NumeroTarjeta);
            cmd.Parameters.AddWithValue("@MetodoDePago", tarjeta.MetodoDePago);
            cmd.Parameters.AddWithValue("@Estado", tarjeta.Activa);

            SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            SqlParameter mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultado);
            cmd.Parameters.Add(mensaje);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value.ToString();
                respuestaAPI.Datos = null;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = ex.Message;
                respuestaAPI.Datos = null;
            }
            finally { conx.Close(); }

            return respuestaAPI;
        }
        public static RespuestaAPI EliminarTarjeta(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarTarjeta", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            SqlParameter mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(mensaje);
            cmd.Parameters.Add(resultado);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value.ToString();
                respuestaAPI.Datos = null;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = ex.Message;
                respuestaAPI.Datos = null;
            }
            finally { conx.Close(); }

            return respuestaAPI;
        }
        public static RespuestaAPI ActualizarTarjeta(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarTarjeta", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            SqlParameter mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultado);
            cmd.Parameters.Add(mensaje);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value.ToString();
                respuestaAPI.Datos = null;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = ex.Message;
                respuestaAPI.Datos = null;
            }
            finally { conx.Close(); }

            return respuestaAPI;
        }
    }
}
