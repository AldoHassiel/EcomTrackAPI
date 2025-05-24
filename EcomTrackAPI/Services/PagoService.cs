using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Services
{
    public class PagoService
    {
        public static List<Pago> ListaDePagos()
        {
            List<Pago> pagos = new List<Pago>();
            SqlConnection conx = Datos.conectar();

            string query = "select Id, Pedido, MetodoPago, ImporteAbonado, FechaPago from Pago";
            SqlCommand command = new SqlCommand(query, conx);

            conx.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Pago pago = new Pago()
                {
                    Id = reader.GetInt32(0),
                    Pedido = reader.GetInt32(1),
                    MetodoPago = reader.GetInt32(2),
                    ImporteAbonado = reader.GetDecimal(3),
                    FechaPago = reader.GetDateTime(4)
                };
                pagos.Add(pago);
            }

            reader.Close();
            command.Dispose();
            conx.Close();

            return pagos;
        }
        public static IActionResult RealizarPago(Pago pago)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("RegistrarPago", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", pago.Id);
            cmd.Parameters.AddWithValue("@Pedido", pago.Pedido);
            cmd.Parameters.AddWithValue("@MetodoPago", pago.MetodoPago);
            cmd.Parameters.AddWithValue("@ImporteAbonado", pago.ImporteAbonado);

            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Pago exitoso");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error al procesar pago: {ex.Message}");
            }
            finally { conx.Close(); }
        }
    }
}
