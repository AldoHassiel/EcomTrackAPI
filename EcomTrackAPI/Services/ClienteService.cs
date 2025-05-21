using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class ClienteService
    {
        public static ClienteRegistroResult RegistrarCliente(ClienteRegistro cliente)
        {
            var resultado = new ClienteRegistroResult();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("RegistrarCliente", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Correo", cliente.Correo);
            cmd.Parameters.AddWithValue("@Pwd", cliente.Pwd);
            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);

            var nuevoIdParam = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(nuevoIdParam);
            var mensajeErrorParam = new SqlParameter("@MensajeError", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(mensajeErrorParam);
            var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Bit){ Direction = ParameterDirection.Output};
            cmd.Parameters.Add(resultadoParam);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                resultado.Resultado = (bool)resultadoParam.Value;
                resultado.IdCliente = resultadoParam.Value != DBNull.Value ? (int?)nuevoIdParam.Value : null;
                resultado.Mensaje = mensajeErrorParam.Value?.ToString();
            }
            catch (SqlException ex)
            {
                resultado.Resultado = false;
                resultado.Mensaje = $"Error en base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                resultado.Resultado = false;
                resultado.Mensaje = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                conx.Close();
            }

            return resultado;
        }
        public static ClienteActualizarResult ActualizarCliente(ClienteActualizar cliente)
        {
            var resultado = new ClienteActualizarResult();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarCliente", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", cliente.Id);
            cmd.Parameters.AddWithValue("@Correo", cliente.Correo);
            cmd.Parameters.AddWithValue("@Pwd", cliente.Pwd);
            cmd.Parameters.AddWithValue("@Estado", cliente.Estado == "Activo" ? 1 : 0);
            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
            cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);

            var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultadoParam);
            var mensajeErrorParam = new SqlParameter("@MensajeError", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(mensajeErrorParam);
            
            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                resultado.Resultado = (bool)resultadoParam.Value;
                resultado.Mensaje = mensajeErrorParam.Value?.ToString();

            }
            catch (SqlException ex)
            {
                resultado.Resultado = false;
                resultado.Mensaje = $"Error en base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                resultado.Resultado = false;
                resultado.Mensaje = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                conx.Close();
            }

            return resultado;
        }
        public static Cliente ObtenerClientePorId(int id)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerClientePorId", conx);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            conx.Open();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;

                return new Cliente
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
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en base de datos: {ex.Message}");
            }
            finally
            {
                conx.Close();
            }
        }
        public static List<Cliente> ObtenerClientes(int orden)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerClientes", conx);
            cmd.Parameters.AddWithValue("@TipoOrden", orden);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Cliente> clientes = new List<Cliente>();
                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Correo = reader.GetString(reader.GetOrdinal("Correo")),
                        Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                        TotalCompras = reader.GetInt32(reader.GetOrdinal("TotalCompras")),
                        TotalGastado = reader.GetDecimal(reader.GetOrdinal("TotalGastado")),
                        Estado = reader.GetBoolean(reader.GetOrdinal("Estado")) ? "Activo" : "Inactivo"
                    });
                }
                return clientes;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en base de datos: {ex.Message}");
            }
            finally
            {
                conx.Close();
            }
        }
        public static ClienteEliminarResult EliminarCliente(int id)
        {
            var resultado = new ClienteEliminarResult();
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarCliente", conx);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultadoParam);
            var mensajeErrorParam = new SqlParameter("@MensajeError", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(mensajeErrorParam);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();
                resultado.Resultado = (bool)resultadoParam.Value;
                resultado.Mensaje = mensajeErrorParam.Value?.ToString();
            }
            catch (SqlException ex)
            {
                resultado.Resultado = false;
                resultado.Mensaje = $"Error en base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                resultado.Resultado = false;
                resultado.Mensaje = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                conx.Close();
            }
            return resultado;
        }
    }
}
