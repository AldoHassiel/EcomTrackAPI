using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class AdminServices
    {
        public static RespuestaAPI RegistrarAdmin(AdminRegistro admin)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarAdministrador", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Correo", admin.Correo.Trim());
            cmd.Parameters.AddWithValue("@Pwd", admin.Pwd.Trim());
            cmd.Parameters.AddWithValue("@Nombre", admin.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Telefono", admin.Telefono.Trim());

            var nuevoId = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var mensaje = new SqlParameter("@MensajeError", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            var resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

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
        public static RespuestaAPI ActualizarAdmin(Admin admin)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarAdministrador", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", admin.Id);
            cmd.Parameters.AddWithValue("@Correo", admin.Correo.Trim());
            cmd.Parameters.AddWithValue("@Pwd", admin.Pwd.Trim());
            cmd.Parameters.AddWithValue("@Estado", admin.Estado == "Activo" ? 1 : 0);
            cmd.Parameters.AddWithValue("@Nombre", admin.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Telefono", admin.Telefono.Trim());

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
        public static RespuestaAPI EliminarAdmin(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarAdministrador", conx);
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
        public static RespuestaAPI ObtenerAdmins()
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerAdministradores", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            
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

                List<Admin> admins = new List<Admin>();

                while (reader.Read())
                {
                    admins.Add(new Admin
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Correo = reader.GetString(reader.GetOrdinal("Correo")),
                        Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                        Estado = reader.GetBoolean(reader.GetOrdinal("Estado")) ? "Activo" : "Inactivo",
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                    });
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Administradores obtenidos correctamente";
                respuestaAPI.Datos = admins;
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
        public static RespuestaAPI ObtenerUnAdmin(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerAdministradorPorId", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            try
            {
                conx.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No se encontró el administrador";
                    respuestaAPI.Datos = null;
                    return respuestaAPI;
                }

                Admin admin = new Admin
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    Correo = reader.GetString(reader.GetOrdinal("Correo")),
                    Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                    Estado = reader.GetBoolean(reader.GetOrdinal("Estado")) ? "Activo" : "Inactivo",
                    FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                };

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Administrador obtenido correctamente";
                respuestaAPI.Datos = admin;

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
    }
}
