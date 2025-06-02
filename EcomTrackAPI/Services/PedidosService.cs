using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;


namespace EcomTrackAPI.Services
{
    public class PedidosService
    {
        public static RespuestaAPI ObtenerPedidos(int id)
        {

            var respuestaAPI = new RespuestaAPI();

            List<Pedidos> pedidos = new List<Pedidos>();

            SqlConnection conx = Datos.conectar();

            string query = "SELECT p.Id, c.Nombre AS Cliente,p.FechaPedido,p.Importe,p.Estado," +
                "p.Calle, p.EntreCalles, p.Numero, p.Colonia, p.Ciudad, p.EstadoD, " +
                "p.CodigoPostal, p.Pais FROM Pedido p JOIN Cliente c ON p.Cliente = c.Id; ";
            SqlCommand command = new SqlCommand(query, conx);

            try
            {

                conx.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Pedidos pedido = new Pedidos()
                    {
                        Id = reader.GetInt32(0),
                        NombreCliente = reader.GetString(1),
                        FechaPedido = reader.GetDateTime(2),
                        Importe = reader.GetDecimal(3),
                        Estado = reader.GetInt32(4),
                    };
                    pedidos.Add(pedido);
                }

                reader.Close();
                command.Dispose();
                conx.Close();

                if (pedidos.Count == 0)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No se encontraron pedidos";
                    respuestaAPI.Datos = null;
                    return respuestaAPI;
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Pedidos obtenidas correctamente";
                respuestaAPI.Datos = pedidos;


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
        public static RespuestaAPI ActualizarEstadoPedido(Pedidos pedido)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarPedido", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", pedido.Id);
            cmd.Parameters.AddWithValue("@Estado", pedido.Estado);

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
        public static RespuestaAPI RegistrarPedido(Pedidos pedido)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();

            SqlCommand cmd = new SqlCommand("RegistrarPedido", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", pedido.Id);
            cmd.Parameters.AddWithValue("@Cliente", pedido.Cliente);
            cmd.Parameters.AddWithValue("@Importe", pedido.Importe);
            cmd.Parameters.AddWithValue("@Estado", pedido.Estado);
          

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
    }
}
