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

        }
        public static IActionResult ActualizarEstadoPedido(Pedidos pedido)
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
