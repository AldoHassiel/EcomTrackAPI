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
        public static RespuestaAPI ObtenerPedidosPeriodo(int FechaInicio = 0, int FechaFin = 0, int Estado = 0)
        {
            var respuesta = new RespuestaAPI();
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerVentasPeriodo", conx);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FechaInicio", FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", FechaFin);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                var calculo = new PedidoPeriodo();
                var pedidosList = new List<PedidoPeriodoList>();

                if (reader.Read())
                {
                    calculo.TotalVenta = reader.IsDBNull(reader.GetOrdinal("TotalVenta")) ? 0 : reader.GetDecimal(reader.GetOrdinal("TotalVenta"));
                    calculo.TotalPedidos = reader.IsDBNull(reader.GetOrdinal("TotalPedidos")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalPedidos"));
                    calculo.Promedio = reader.IsDBNull(reader.GetOrdinal("Promedio")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Promedio"));
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var pedido = new PedidoPeriodoList
                        {
                            IdPedido = reader.GetInt32(reader.GetOrdinal("IdPedido")),
                            IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                            NombreCliente = reader.GetString(reader.GetOrdinal("NombreCliente")),
                            FechaPedido = reader.GetDateTime(reader.GetOrdinal("FechaPedido")),
                            Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                            MarcaTarjeta = reader.GetString(reader.GetOrdinal("MarcaTarjeta")),
                            Estado = reader.GetString(reader.GetOrdinal("Estado"))
                        };
                        pedidosList.Add(pedido);
                    }
                }

                if (pedidosList.Count == 0)
                {
                    respuesta.Resultado = false;
                    respuesta.Mensaje = "No se encontraron pedidos en el periodo especificado.";
                    respuesta.Datos = null;
                    return respuesta;
                }
                respuesta.Resultado = true;
                respuesta.Mensaje = "Pedidos obtenidos correctamente.";
                respuesta.Datos = new { Calculo = calculo, Pedidos = pedidosList };
            }
            catch (Exception ex)
            {
                respuesta.Resultado = false;
                respuesta.Mensaje = "Error al obtener pedidos del periodo: " + ex.Message;
                respuesta.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuesta;
        }
    }
}
