using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;


namespace EcomTrackAPI.Services
{
    public class PedidosService
    {
        public static List<Pedidos> ObtenerPedidos()
        {
            List<Pedidos> pedidos = new List<Pedidos>();
            SqlConnection conx = Datos.conectar();

            string query = "select Id, Cliente, FechaPedido, Importe, Estado, Calle," +
                " EntreCalles, Numero, Colonia, Ciudad, EstadoD, CodigoPostal, Pais from Pedido";
            SqlCommand command = new SqlCommand(query, conx);

            conx.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Pedidos pedido = new Pedidos()
                {
                    Id = reader.GetInt32(0),
                    Cliente = reader.GetInt32(1),
                    FechaPedido = reader.GetDateTime(2),
                    Importe = reader.GetDecimal(3),
                    Estado = reader.GetInt32(4),
                    Calle = reader.GetString(5),
                    EntreCalles = reader.GetString(6),
                    Numero = reader.GetString(7),
                    Colonia = reader.GetString(8),
                    Ciudad = reader.GetString(9),
                    EstadoD = reader.GetString(10),
                    CodigoPostal = reader.GetString(11),
                    Pais = reader.GetString(12),

                };
                pedidos.Add(pedido);
            }

            reader.Close();
            command.Dispose();
            conx.Close();

            return pedidos;
        }
        public static IActionResult ActualizarEstadoPedido(Pedidos pedido)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("RegistrarPedido", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", pedido.Id);
            cmd.Parameters.AddWithValue("@Estado", pedido.Estado);

            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Pedido actualizado");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error al actualizar pedido: {ex.Message}");
            }
            finally { conx.Close(); }
        }
        public static IActionResult RegistrarPedido(Pedidos pedido)
        {
            SqlConnection conx = Datos.conectar();

            SqlCommand cmd = new SqlCommand("RegistrarPedidos", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", pedido.Id);
            cmd.Parameters.AddWithValue("@Cliente", pedido.Cliente);
            cmd.Parameters.AddWithValue("@Importe", pedido.Importe);
            cmd.Parameters.AddWithValue("@Estado", pedido.Estado);


            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Pedido realizado");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error al realizar pedido: {ex.Message}");
            }
            finally { conx.Close(); }
        }
    }
}
