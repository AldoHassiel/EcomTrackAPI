using EcomTrackAPI.Models;
using EcomTrackAPI.Models.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class CarritoService
    {
        public static RespuestaAPI AgregarProductoAlCarrito(AgregarProductoCarritoDTO dto)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("AgregarProductoAlCarrito", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdCliente", dto.Cliente);
            cmd.Parameters.AddWithValue("@IdProducto", dto.Producto);
            cmd.Parameters.AddWithValue("@Cantidad", dto.Cantidad);

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
                respuestaAPI.Mensaje = "Error al agregar producto al carrito: " + ex.Message;
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }

        public static RespuestaAPI ActualizarCantidadProductoCarrito(ActualizarCantidadProductoDTO dto)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarCantidadProductoDelCarrito", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdCliente", dto.Cliente);
            cmd.Parameters.AddWithValue("@IdProducto", dto.Producto);
            cmd.Parameters.AddWithValue("@Cantidad", dto.Cantidad);

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
                respuestaAPI.Mensaje = "Error al actualizar cantidad del producto: " + ex.Message;
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
        public static RespuestaAPI EliminarProductoDelCarrito(EliminarProductoCarritoDTO dto)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarProductoDelCarrito", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdCliente", dto.Cliente);
            cmd.Parameters.AddWithValue("@IdProducto", dto.Producto);

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
                respuestaAPI.Mensaje = "Error al eliminar producto del carrito: " + ex.Message;
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
        public static RespuestaAPI ObtenerCarrito(int idCliente)
        {
            var respuestaAPI = new RespuestaAPI();
            var carritoDTO = new CarritoDTO();
            var productosCarrito = new List<ProductoCarritoDTO>();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ObtenerCarrito", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdCliente", idCliente);

            SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            SqlParameter mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultado);
            cmd.Parameters.Add(mensaje);

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // Leer la información del carrito
                if (reader.Read())
                {
                    carritoDTO.Total = Convert.ToDecimal(reader["Total"]);
                    carritoDTO.CantidadProductos = Convert.ToInt32(reader["CantidadProductos"]);
                    carritoDTO.EstaVacio = Convert.ToBoolean(reader["EstaVacio"]);
                }

                // Avanzar al siguiente conjunto de resultados (productos)
                reader.NextResult();

                // Leer los productos del carrito
                while (reader.Read())
                {
                    var producto = new ProductoCarritoDTO
                    {
                        Id = reader["IdDetalle"] != DBNull.Value ? Convert.ToInt32(reader["IdDetalle"]) : 0,
                        Producto = reader["IdProducto"] != DBNull.Value ? Convert.ToInt32(reader["IdProducto"]) : 0,
                        ProductoNombre = reader["ProductoNombre"] != DBNull.Value ? reader["ProductoNombre"].ToString() : string.Empty,
                        Cantidad = reader["Cantidad"] != DBNull.Value ? Convert.ToInt32(reader["Cantidad"]) : 0,
                        Precio = reader["PrecioUnitario"] != DBNull.Value ? Convert.ToDecimal(reader["PrecioUnitario"]) : 0,
                        Subtotal = reader["Subtotal"] != DBNull.Value ? Convert.ToDecimal(reader["Subtotal"]) : 0,
                        UrlImagen = reader["UrlImagen"] != DBNull.Value ? reader["UrlImagen"].ToString() : string.Empty
                    };

                    productosCarrito.Add(producto);
                }

                reader.Close();

                carritoDTO.Productos = productosCarrito;

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value.ToString();
                respuestaAPI.Datos = carritoDTO;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = "Error al obtener carrito: " + ex.Message;
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
    }
}
