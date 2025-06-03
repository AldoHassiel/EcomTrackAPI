using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;
using EcomTrackAPI.Models.DTO;

namespace EcomTrackAPI.Services
{
    public class PedidosService
    {
        public static RespuestaAPI ObtenerPedidosDelCliente(int IdCliente)
        {
            var respuesta = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerPedidosDelCliente", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IdCliente", IdCliente);

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                var pedidos = new List<PedidoClienteDTO>();

                while (reader.Read())
                {
                    var pedido = new PedidoClienteDTO
                    {
                        Id = reader.GetInt32(0),
                        FechaPedido = reader.GetDateTime(1),
                        Total = reader.GetDecimal(2),
                        Estado = reader.GetString(3)
                    };
                    pedidos.Add(pedido);
                }

                if (pedidos.Count == 0)
                {
                    respuesta.Resultado = false;
                    respuesta.Mensaje = "No se encontraron pedidos para el cliente.";
                    respuesta.Datos = null;
                    return respuesta;
                }

                respuesta.Resultado = true;
                respuesta.Mensaje = "Pedidos obtenidos correctamente.";
                respuesta.Datos = pedidos;
            }
            catch (Exception ex)
            {
                respuesta.Resultado = false;
                respuesta.Mensaje = "Error al obtener pedidos del cliente: " + ex.Message;
                respuesta.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuesta;
        }
        public static RespuestaAPI ObtenerDetallePedidoCliente(int IdCliente, int IdPedido)
        {
            var respuesta = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();

            SqlCommand cmd = new SqlCommand("ObtenerDetallePedidoCliente", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
            cmd.Parameters.AddWithValue("@IdPedido", IdPedido);

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                var detalle = new PedidoDetalleClienteDTO();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        detalle.Productos.Add(new ProductosPedidoDTO
                        {
                            IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                            NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                            Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                            SubtotalProducto = reader.GetDecimal(reader.GetOrdinal("SubtotalProducto")),
                            UrlImagen = reader.GetString(reader.GetOrdinal("UrlImagen"))
                        });
                    }
                }

                if (reader.NextResult() && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        detalle.NumeroTarjeta = reader["NumeroTarjeta"]?.ToString();
                        detalle.MetodoPago = reader["MetodoDePago"]?.ToString();
                        detalle.MarcaTarjeta = reader["MarcaTarjeta"]?.ToString();
                    }
                }

                if (reader.NextResult() && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        detalle.NombreTipoDireccion = reader["NombreTipoDireccion"]?.ToString();
                        detalle.Calle = reader["Calle"]?.ToString();
                        detalle.Numero = reader["Numero"]?.ToString();
                        detalle.EntreCalles = reader["EntreCalles"]?.ToString();
                        detalle.Colonia = reader["Colonia"]?.ToString();
                        detalle.Ciudad = reader["Ciudad"]?.ToString();
                        detalle.EstadoDireccion = reader["EstadoDireccion"]?.ToString();
                        detalle.CodigoPostal = reader["CodigoPostal"]?.ToString();
                    }
                }

                if (reader.NextResult() && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        detalle.EstadoPedido = reader["EstadoPedido"]?.ToString();
                        detalle.Importe = reader["Importe"] != DBNull.Value ? Convert.ToDecimal(reader["Importe"]) : 0;
                        detalle.Transportista = reader["Transportista"]?.ToString();
                    }
                }

                respuesta.Resultado = true;
                respuesta.Mensaje = "Detalle del pedido obtenido correctamente";
                respuesta.Datos = detalle;
            }
            catch (Exception ex)
            {
                respuesta.Resultado = false;
                respuesta.Mensaje = "Error al obtener detalles del pedido: " + ex.Message;
                respuesta.Datos = null;
            }
            finally
            {
                conx.Close();
            }
            return respuesta;
        }


        public static RespuestaAPI ObtenerIngresosTotales()
        {
            var respuestaAPI = new RespuestaAPI();

            using (SqlConnection conx = Datos.conectar())
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conx;
                cmd.CommandText = "select SUM(Importe) from Pedido";
                cmd.CommandType = CommandType.Text;

                try
                {
                    conx.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        respuestaAPI.Datos = reader.GetDecimal(0);
                        respuestaAPI.Resultado = true;

                    }
                }
                catch (Exception ex)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
                }
                return respuestaAPI;
            }
               
        }

    }
}
