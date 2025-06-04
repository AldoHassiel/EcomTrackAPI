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

            string query = "SELECT e.Id, e.FechaEnvio, e.FechaEntrega, e.IdPedido, e.IdTransportista, " +
                "t.Nombre AS NombreTransportista, e.IdEstado, es.Descripcion AS EstadoDescripcion "  +
                "FROM Envio e LEFT OUTER JOIN Transportista t ON e.IdTransportista = t.Id " +
                "LEFT OUTER JOIN Estado es ON e.IdEstado = es.Id;";
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
                    NombreTransportista = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    IdEstado = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                    DescripcionEstado = reader.IsDBNull(7) ? "" : reader.GetString(7),
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
            using (SqlConnection conx = Datos.conectar())
            using (SqlCommand cmd = new SqlCommand("ActualizarEnvio", conx))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("@Id", envio.Id);
                cmd.Parameters.AddWithValue("@IdEstado", envio.IdEstado);

                // Parámetros de salida
                SqlParameter resultadoParam = new SqlParameter("@Resultado", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(resultadoParam);

                SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(mensajeParam);

                try
                {
                    conx.Open();
                    cmd.ExecuteNonQuery();

                    bool resultado = Convert.ToBoolean(resultadoParam.Value);
                    string mensaje = mensajeParam.Value.ToString();

                    if (resultado)
                        return new OkObjectResult(mensaje);
                    else
                        return new BadRequestObjectResult(mensaje);
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult($"Error en la API al actualizar el envío: {ex.Message}");
                }
            }
        }

        public static IActionResult ActualizarEnvioTransportista(Envio envio)
        {
            using (SqlConnection conx = Datos.conectar())
            using (SqlCommand cmd = new SqlCommand("ActualizarEnvioTransportista", conx))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("@Id", envio.Id);
                cmd.Parameters.AddWithValue("@IdTransportista", envio.IdTransportista);
                cmd.Parameters.AddWithValue("@FechaEnvio", DateTime.Now);


                // Parámetros de salida
                SqlParameter resultadoParam = new SqlParameter("@Resultado", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(resultadoParam);

                SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(mensajeParam);

                try
                {
                    conx.Open();
                    cmd.ExecuteNonQuery();

                    bool resultado = Convert.ToBoolean(resultadoParam.Value);
                    string mensaje = mensajeParam.Value.ToString();

                    if (resultado)
                        return new OkObjectResult(mensaje);
                    else
                        return new BadRequestObjectResult(mensaje);
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult($"Error en la API al actualizar el transportista: {ex.Message}");
                }
            }
        }


        public static IActionResult ObtenerEstadoPorEnvio(int envioId)
        {
            using (SqlConnection conx = Datos.conectar())
            using (SqlCommand cmd = new SqlCommand("ObtenerEstadoPorEnvio", conx))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetro de entrada
                cmd.Parameters.AddWithValue("@IdEnvio", envioId);

                // Parámetros de salida
                SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                SqlParameter mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(resultado);
                cmd.Parameters.Add(mensaje);

                try
                {
                    conx.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string descripcionEstado = reader["EstadoDescripcion"].ToString();

                            return new OkObjectResult(new
                            {
                                Estado = descripcionEstado,
                                Mensaje = mensaje.Value?.ToString()
                            });
                        }
                        else
                        {
                            return new NotFoundObjectResult("No se encontró el estado para el envío especificado.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult($"Error al consultar el estado del envío: {ex.Message}");
                }


            }
        }

    }
}
