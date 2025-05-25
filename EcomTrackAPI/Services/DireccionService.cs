using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class DireccionService
    {
        public static RespuestaAPI ListaDirecciones()
        {
            var respuestaAPI = new RespuestaAPI();

            List<Direccion> direcciones = new List<Direccion>();

            SqlConnection conx = Datos.conectar();

            string query = "select Id, Cliente, Tipo, Calle, EntreCalles, Numero, Colonia, " +
                "Ciudad, Estado, CodigoPostal, Pais from Direccion";
            SqlCommand command = new SqlCommand(query, conx);

            try
            {
                conx.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Direccion direccion = new Direccion()
                    {
                        Id = reader.GetInt32(0),
                        Cliente = reader.GetInt32(1),
                        Tipo = reader.GetInt32(2),
                        Calle = reader.GetString(3),
                        EntreCalles = reader.GetString(4),
                        Numero = reader.GetString(5),
                        Colonia = reader.GetString(6),
                        Ciudad = reader.GetString(7),
                        Estado = reader.GetString(8),
                        CodigoPostal = reader.GetString(9),
                        Pais = reader.GetString(10),
                    };
                    direcciones.Add(direccion);
                }

                reader.Close();
                command.Dispose();
                conx.Close();

                if (direcciones.Count == 0)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No se encontraron direcciones";
                    respuestaAPI.Datos = null;
                    return respuestaAPI;
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Direcciones obtenidas correctamente";
                respuestaAPI.Datos = direcciones;
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
        public static RespuestaAPI EditarDireccion(Direccion direccion)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarDireccion", conx);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", direccion.Id);
            cmd.Parameters.AddWithValue("@Cliente", direccion.Cliente);
            cmd.Parameters.AddWithValue("@Tipo", direccion.Tipo);
            cmd.Parameters.AddWithValue("@Calle", direccion.Calle);
            cmd.Parameters.AddWithValue("@EntreCalles", direccion.EntreCalles);
            cmd.Parameters.AddWithValue("@Numero", direccion.Numero);
            cmd.Parameters.AddWithValue("@Colonia", direccion.Colonia);
            cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
            cmd.Parameters.AddWithValue("@Estado", direccion.Estado);
            cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);
            cmd.Parameters.AddWithValue("@Pais", direccion.Pais);

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
        public static RespuestaAPI GuardarDireccion(Direccion direccion)
        {

            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarDireccion", conx);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", direccion.Id);
            cmd.Parameters.AddWithValue("@Cliente", direccion.Cliente);
            cmd.Parameters.AddWithValue("@Tipo", direccion.Tipo);
            cmd.Parameters.AddWithValue("@Calle", direccion.Calle);
            cmd.Parameters.AddWithValue("@EntreCalles", direccion.EntreCalles);
            cmd.Parameters.AddWithValue("@Numero", direccion.Numero);
            cmd.Parameters.AddWithValue("@Colonia", direccion.Colonia);
            cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
            cmd.Parameters.AddWithValue("@Estado", direccion.Estado);
            cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);
            cmd.Parameters.AddWithValue("@Pais", direccion.Pais);


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
        public static RespuestaAPI EliminarDireccion(int id)
        {
            var respuestaAPI = new RespuestaAPI();
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarDireccion", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            SqlParameter mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

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
