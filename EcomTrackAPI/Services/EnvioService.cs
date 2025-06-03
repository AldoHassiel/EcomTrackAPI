using EcomTrackAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class EnvioService
    {
        public static List<Envio> ObtenerEnvios()
        {
            List<Envio> envios = new List<Envio>();
            SqlConnection conx = Datos.conectar();

            string query = "SELECT Id, FechaEnvio, FechaEntrega, IdPedido, IdTransportista, IdEstado FROM Envio";
            SqlCommand command = new SqlCommand(query, conx);

            conx.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Envio envio = new Envio()
                {
                    Id = reader.GetInt32(0),
                    FechaEnvio = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1),
                    FechaEntrega = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                    IdPedido = reader.GetInt32(3),
                    IdTransportista = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                    IdEstado = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5)
                };
                envios.Add(envio);
            }

            reader.Close();
            command.Dispose();
            conx.Close();

            return envios;
        }
        public static IActionResult GuardarEnvio(Envio envio)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarEnvio", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", envio.Id);
            cmd.Parameters.AddWithValue("@FechaEnvio", envio.FechaEnvio);
            cmd.Parameters.AddWithValue("@FechaEntrega", envio.FechaEnvio);
            cmd.Parameters.AddWithValue("@IdPedido", envio.IdPedido);
            cmd.Parameters.AddWithValue("@IdTransportista", envio.IdTransportista);
            cmd.Parameters.AddWithValue("@IdEstado", envio.IdEstado);

            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Envio realizado");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"No se pudo realizar el envío: {ex.Message}");
            }
            finally { conx.Close(); }
        }

        public static IActionResult ActualizarEnvio(Envio envio)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarEnvio", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", envio.Id);
            cmd.Parameters.AddWithValue("@IdEstado", envio.IdEstado);

            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Estado del envío actualizado");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"No se pudo actualizar el estado del envío: {ex.Message}");
            }
            finally { conx.Close(); }
        }

        public static IActionResult ObtenerEstadoPorEnvio(int envioId)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerEstadoPorEnvio", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EnvioId", envioId);

            conx.Open();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Leer el resultado del procedimiento almacenado
                    string descripcionEstado = reader["Descripcion"].ToString();
                    return new OkObjectResult(descripcionEstado);
                }
                else
                {
                    return new NotFoundObjectResult("No se encontró el estado para el envío especificado.");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"No se pudo realizar la consulta: {ex.Message}");
            }
            finally
            {
                conx.Close();
            }
        }

    }
}
