using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class PagoService
    {
        public static RespuestaAPI ListaDePagos()
        {
            var respuestaAPI = new RespuestaAPI();

            List<Pago> pagos = new List<Pago>();
            SqlConnection conx = Datos.conectar();

            string query = "select Id, Pedido, MetodoPago, ImporteAbonado, FechaPago from Pago";
            SqlCommand command = new SqlCommand(query, conx);

            try { 
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

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Pagos obtenidos correctamente";
                respuestaAPI.Datos = pagos;

                reader.Close();
                command.Dispose();
                

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
        public static RespuestaAPI RealizarPago(Pago pago)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("RegistrarPago", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", pago.Id);
            cmd.Parameters.AddWithValue("@Pedido", pago.Pedido);
            cmd.Parameters.AddWithValue("@MetodoPago", pago.MetodoPago);
            cmd.Parameters.AddWithValue("@ImporteAbonado", pago.ImporteAbonado);

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
