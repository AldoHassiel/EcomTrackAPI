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
                    Importe = reader.GetDecimal(2),
                    Estado = reader.GetInt32(3),
                    Calle = reader.GetString(4),
                    EntreCalles = reader.GetString(5),
                    Numero = reader.GetString(6),
                    Colonia = reader.GetString(7),
                    Ciudad = reader.GetString(8),
                    EstadoD = reader.GetString(9),
                    CodigoPostal = reader.GetString(10),
                    Pais = reader.GetString(11),

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
