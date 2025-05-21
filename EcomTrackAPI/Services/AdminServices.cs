using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class AdminServices
    {
        public static ResultadoRegistro RegistrarAdmin(AdminRegistro admin)
        {
            var resultado = new ResultadoRegistro();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("RegistrarAdministrador", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Correo", admin.Correo.Trim());
            cmd.Parameters.AddWithValue("@Pwd", admin.Pwd.Trim());
            cmd.Parameters.AddWithValue("@Nombre", admin.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Telefono", admin.Telefono.Trim());

            var nuevoIdParam = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(nuevoIdParam);
            var mensajeErrorParam = new SqlParameter("@MensajeError", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(mensajeErrorParam);
            var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultadoParam);

            try
            {
                conx.Open();
                cmd.ExecuteNonQuery();

                resultado.Resultado = (bool)resultadoParam.Value;
                resultado.IdUsuario = resultadoParam.Value != DBNull.Value ? (int?)nuevoIdParam.Value : null;
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
        public static ResultadoPeticion ActualizarAdmin(Admin admin)
        {
            var resultado = new ResultadoPeticion();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarAdministrador", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", admin.Id);
            cmd.Parameters.AddWithValue("@Correo", admin.Correo.Trim());
            cmd.Parameters.AddWithValue("@Pwd", admin.Pwd.Trim());
            cmd.Parameters.AddWithValue("@Estado", admin.Estado == "Activo" ? 1 : 0);
            cmd.Parameters.AddWithValue("@Nombre", admin.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Telefono", admin.Telefono.Trim());

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
        public static ResultadoPeticion EliminarAdmin(int id)
        {
            var resultado = new ResultadoPeticion();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarAdministrador", conx);
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
        public static List<Admin> ObtenerAdmins()
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerAdministradores", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            List<Admin> admins = new List<Admin>();
            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();
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
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en base de datos: {ex.Message}");
            }
            finally
            {
                conx.Close();
            }

            return admins;
        }
        public static Admin ObtenerAdminPorId(int id)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerAdministradorPorId", conx);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            Admin admin = null;

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    admin = new Admin
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Correo = reader.GetString(reader.GetOrdinal("Correo")),
                        Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                        Estado = reader.GetBoolean(reader.GetOrdinal("Estado")) ? "Activo" : "Inactivo",
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                    };
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en base de datos: {ex.Message}");
            }
            finally
            {
                conx.Close();
            }

            return admin;
        }
    }
}
