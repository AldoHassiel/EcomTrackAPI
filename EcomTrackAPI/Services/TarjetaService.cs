using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;
using System;

namespace EcomTrackAPI.Services
{
    public class TarjetaService
    {
        public static RespuestaAPI ObtenerTarjetas(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            List<Tarjeta> tarjetas = new List<Tarjeta>();

            SqlConnection conx = Datos.conectar();
            string query = "SELECT t.Id, t.NumeroTarjeta, t.MetodoDePago, m.Descripcion AS TipoTarjeta, t.MarcaTarjeta, t.Activa, t.Predeterminada FROM Tarjeta t INNER JOIN MetodoPago m ON m.Id = t.MetodoDePago WHERE t.Cliente = " +id +" AND t.Activa = 1 ORDER BY t.Predeterminada DESC";

            SqlCommand cmd = new SqlCommand(query, conx);

            try
            {
                conx.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                { 
                    Tarjeta tarjeta = new Tarjeta()
                    {
                        Id = reader.GetInt32(0),
                        NumeroTarjeta = reader.GetString(1),
                        MetodoDePago = reader.GetInt32(2),
                        TipoTarjeta = reader.GetString(3), 
                        MarcaTarjeta = reader.GetString(4),
                        Activa = reader.GetBoolean(5),
                        Predeterminada = reader.GetBoolean(6)
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
            cmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = tarjeta.Cliente;
            cmd.Parameters.Add("@NumeroTarjeta", SqlDbType.VarChar, 16).Value = tarjeta.NumeroTarjeta;
            cmd.Parameters.Add("@MetodoDePago", SqlDbType.Int).Value = tarjeta.MetodoDePago;
            cmd.Parameters.Add("@MarcaTarjeta", SqlDbType.VarChar, 100).Value = tarjeta.MarcaTarjeta;
            cmd.Parameters.Add("@Activa", SqlDbType.Bit).Value = tarjeta.Activa;
            cmd.Parameters.Add("@Predeterminada", SqlDbType.Bit).Value = tarjeta.Predeterminada;

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
