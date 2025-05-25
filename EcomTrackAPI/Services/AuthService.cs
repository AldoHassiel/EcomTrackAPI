using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class AuthService
    {
        public static RespuestaAPI Login(Login login)
        {
            var sesion = new RespuestaAPI();

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
                    sesion.Resultado = reader.GetBoolean(reader.GetOrdinal("Autenticado"));
                    sesion.Mensaje = reader.GetString(reader.GetOrdinal("Mensaje"));
                    int IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario"));
                    bool EsAdmin = reader.GetBoolean(reader.GetOrdinal("EsAdmin"));

                    if (sesion.Resultado)
                    {
                        if (EsAdmin)
                        {
                            var admin = AdminServices.ObtenerUnAdmin(IdUsuario).Datos;
                            sesion.Datos = new Sesion() { EsAdmin = true, Usuario = admin };
                           
                        }
                        else
                        {
                            var cliente = ClienteService.ObtenerUnCliente(IdUsuario).Datos;
                            sesion.Datos = new Sesion() { EsAdmin = false, Usuario = cliente };
                        }
                    }

                    return sesion;
                }

                sesion.Resultado = false;
                sesion.Mensaje = "No se encontró el usuario o la contraseña es incorrecta";
                sesion.Datos = null;
            }
            catch (SqlException ex)
            {
                sesion.Resultado = false;
                sesion.Mensaje = $"Error en base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                sesion.Resultado = false;
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
