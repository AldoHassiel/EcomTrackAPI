using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;

namespace EcomTrackAPI.Services
{
    public static class ProductoService
    {
        public static RespuestaAPI GuardarProducto(Producto producto)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarProducto", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion.Trim());
            cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
            cmd.Parameters.AddWithValue("@Precio", producto.Precio);
            cmd.Parameters.AddWithValue("@Stock", producto.Stock);
            cmd.Parameters.AddWithValue("@Proveedor", producto.Proveedor);
            cmd.Parameters.AddWithValue("@UrlImagen", producto.UrlImagen.Trim());
            cmd.Parameters.AddWithValue("@Estado", producto.Estado);

            var mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            var resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(mensaje);
            cmd.Parameters.Add(resultado);

            try
            {
                conx.Open();

                cmd.ExecuteNonQuery();

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value?.ToString();
                respuestaAPI.Datos = null;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
        public static RespuestaAPI ActualizarProducto(Producto producto)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarProducto", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", producto.Id);
            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion.Trim());
            cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
            cmd.Parameters.AddWithValue("@Precio", producto.Precio);
            cmd.Parameters.AddWithValue("@Stock", producto.Stock);
            cmd.Parameters.AddWithValue("@Proveedor", producto.Proveedor);
            cmd.Parameters.AddWithValue("@UrlImagen", producto.UrlImagen.Trim());
            cmd.Parameters.AddWithValue("@Estado", producto.Estado);

            var mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            var resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(mensaje);
            cmd.Parameters.Add(resultado);

            try
            {
                conx.Open();

                cmd.ExecuteNonQuery();

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value?.ToString();
                respuestaAPI.Datos = null;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
        public static RespuestaAPI EliminarProducto(int id)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("EliminarProducto", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            var mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            var resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(mensaje);
            cmd.Parameters.Add(resultado);

            try
            {
                conx.Open();

                cmd.ExecuteNonQuery();

                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value?.ToString();
                respuestaAPI.Datos = null;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
                respuestaAPI.Datos = null;
            }
            finally
            {
                conx.Close();
            }

            return respuestaAPI;
        }
        public static RespuestaAPI ObtenerProductos()
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();

            string query = "select Id, Nombre, Descripcion, Categoria, Precio, Stock, Proveedor, UrlImagen, FechaRegistro, Estado from Producto";
            SqlCommand cmd = new SqlCommand(query, conx);

            try
            {
                conx.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    respuestaAPI.Resultado = false;
                    respuestaAPI.Mensaje = "No se encontraron productos";
                    respuestaAPI.Datos = null;
                    return respuestaAPI;
                }

                List<Producto> productos = new List<Producto>();

                while (reader.Read())
                {
                    Producto producto = new Producto
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        Categoria = reader.GetInt32(3),
                        Precio = reader.GetDecimal(4),
                        Stock = reader.GetInt32(5),
                        Proveedor = reader.GetInt32(6),
                        UrlImagen = reader.GetString(7),
                        FechaRegistro = reader.GetDateTime(8),
                        Estado = reader.GetBoolean(9)
                    };
                    productos.Add(producto);
                }

                respuestaAPI.Resultado = true;
                respuestaAPI.Mensaje = "Productos obtenidos correctamente";
                respuestaAPI.Datos = productos;
            }
            catch (Exception ex)
            {
                respuestaAPI.Resultado = false;
                respuestaAPI.Mensaje = $"Error inesperado: {ex.Message}";
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
