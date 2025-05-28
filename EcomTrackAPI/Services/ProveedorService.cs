using EcomTrackAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class ProveedorService
    {
        public static RespuestaAPI GuardarProveedor(Proveedor proveedor)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarProveedor", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Correo", proveedor.Correo.Trim());
            cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono.Trim());
            cmd.Parameters.AddWithValue("@Estado", proveedor.Estado == "Activo" ? 1 : 0);

            var mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
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
        public static RespuestaAPI ActualizarProveedor(Proveedor proveedor)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarProveedor", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", proveedor.Id);
            cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Correo", proveedor.Correo.Trim());
            cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono.Trim());
            cmd.Parameters.AddWithValue("@Estado", proveedor.Estado == "Activo" ? 1 : 0);

            var mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
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
        public static RespuestaAPI EliminarProveedor(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarProveedor", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            var mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
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
        public static RespuestaAPI ObtenerProveedores()
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            string query = "select Id, Nombre, Correo, Telefono, Estado from Proveedor";
            SqlCommand cmd = new SqlCommand(query, conx);

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No se encontraron proveedores";
                    respuestaAPI.Datos = null;
                    return respuestaAPI;
                }

                List<Proveedor> proveedores = new List<Proveedor>();

                while (reader.Read())
                {
                    Proveedor proveedor = new Proveedor
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Correo = reader.GetString(2),
                        Telefono = reader.GetString(3),
                        Estado = reader.GetBoolean(4) ? "Activo" : "Inactivo"
                    };
                    proveedores.Add(proveedor);
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Proveedores obtenidos correctamente";
                respuestaAPI.Datos = proveedores;
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

        public static RespuestaAPI ObtenerIDyNombre()
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            string query = "select Id, Nombre from Proveedor where Estado = 1";
            SqlCommand cmd = new SqlCommand(query, conx);

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No se encontraron proveedores";
                    respuestaAPI.Datos = null;
                    return respuestaAPI;
                }

                List<Proveedor> proveedores = new List<Proveedor>();

                while (reader.Read())
                {
                    Proveedor proveedor = new Proveedor
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    };
                    proveedores.Add(proveedor);
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Proveedores obtenidos correctamente";
                respuestaAPI.Datos = proveedores;
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
