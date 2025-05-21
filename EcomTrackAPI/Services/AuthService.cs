using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class AuthService
    {
        public static Sesion Login(Login login)
        {
            var sesion = new Sesion();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("IniciarSesion", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Correo", login.Correo);
            cmd.Parameters.AddWithValue("@Pwd", login.Pwd);

            try
            {
                conx.Open();
                var reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    sesion.Autenticado = reader.GetBoolean(reader.GetOrdinal("Autenticado"));
                    sesion.Mensaje = reader.GetString(reader.GetOrdinal("Mensaje"));
                    sesion.IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario"));
                    sesion.EsAdmin = reader.GetBoolean(reader.GetOrdinal("EsAdmin"));
                }
            }
            catch (SqlException ex)
            {
                sesion.Autenticado = false;
                sesion.Mensaje = $"Error en base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                sesion.Autenticado = false;
                sesion.Mensaje = $"Error inesperado: {ex.Message}";
            }
            finally
            {
                conx.Close();
            }

            return sesion;
        }
    }
}
