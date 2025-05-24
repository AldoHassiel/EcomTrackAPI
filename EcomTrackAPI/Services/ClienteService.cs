using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class ClienteService
    {
        public static RespuestaAPI RegistrarCliente(ClienteRegistro cliente)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarCliente", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Correo", cliente.Correo.Trim());
            cmd.Parameters.AddWithValue("@Pwd", cliente.Pwd.Trim());
            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono.Trim());

            var nuevoId = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var mensaje = new SqlParameter("@MensajeError", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            var resultado = new SqlParameter("@Resultado", SqlDbType.Bit){ Direction = ParameterDirection.Output};

            cmd.Parameters.Add(nuevoId);
            cmd.Parameters.Add(mensaje);
            cmd.Parameters.Add(resultado);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value?.ToString();

                respuestaAPI.Datos = nuevoId.Value != DBNull.Value ? Convert.ToInt32(nuevoId.Value) : null;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
        public static RespuestaAPI ActualizarCliente(ClienteActualizar cliente)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarCliente", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", cliente.Id);
            cmd.Parameters.AddWithValue("@Correo", cliente.Correo.Trim());
            cmd.Parameters.AddWithValue("@Pwd", cliente.Pwd.Trim());
            cmd.Parameters.AddWithValue("@Estado", cliente.Estado == "Activo" ? 1 : 0);
            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono.Trim());

            var mensaje = new SqlParameter("@MensajeError", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            var resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };


            cmd.Parameters.Add(mensaje);
            cmd.Parameters.Add(resultado);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value?.ToString();
                respuestaAPI.Datos = null;

            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
        public static RespuestaAPI ObtenerUnCliente(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerClientePorId", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            try
            {
                conx.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No se encontró el cliente";
                    return respuestaAPI;
                }

                var cliente = new Cliente
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    Correo = reader.GetString(reader.GetOrdinal("Correo")),
                    Pwd = reader.GetString(reader.GetOrdinal("Pwd")),
                    Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                    FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                    TotalCompras = reader.GetInt32(reader.GetOrdinal("TotalCompras")),
                    TotalGastado = reader.GetDecimal(reader.GetOrdinal("TotalGastado")),
                    Estado = reader.GetBoolean(reader.GetOrdinal("Estado")) ? "Activo" : "Inactivo"
                };
                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Cliente encontrado";
                respuestaAPI.Datos = cliente;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
        public static RespuestaAPI ObtenerClientes(int orden)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerClientes", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TipoOrden", orden);

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No se encontraron clientes";
                    respuestaAPI.Datos = null;
                    return respuestaAPI;
                }

                List<Cliente> clientes = new List<Cliente>();

                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Correo = reader.GetString(reader.GetOrdinal("Correo")),
                        Pwd = reader.GetString(reader.GetOrdinal("Pwd")),
                        Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                        TotalCompras = reader.GetInt32(reader.GetOrdinal("TotalCompras")),
                        TotalGastado = reader.GetDecimal(reader.GetOrdinal("TotalGastado")),
                        Estado = reader.GetBoolean(reader.GetOrdinal("Estado")) ? "Activo" : "Inactivo"
                    });
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Clientes obtenidos correctamente";
                respuestaAPI.Datos = clientes;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                conx.Close();
            }
            return respuestaAPI;
        }
        public static RespuestaAPI EliminarCliente(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarCliente", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            var resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            var mensaje = new SqlParameter("@MensajeError", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultado);
            cmd.Parameters.Add(mensaje);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value?.ToString();
                respuestaAPI.Datos = null;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
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
