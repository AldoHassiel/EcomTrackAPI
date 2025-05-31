using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;

namespace EcomTrackAPI.Services
{
    public static class ProductoService
    {
        public static RespuestaAPI GuardarProducto(ProductoNuevo producto)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarProducto", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion.Trim());
            cmd.Parameters.AddWithValue("@Categoria", producto.IdCategoria);
            cmd.Parameters.AddWithValue("@Precio", producto.Precio);
            cmd.Parameters.AddWithValue("@Stock", producto.Stock);
            cmd.Parameters.AddWithValue("@Proveedor", producto.IdProveedor);
            cmd.Parameters.AddWithValue("@UrlImagen", producto.UrlImagen.Trim());
            cmd.Parameters.AddWithValue("@Estado", producto.Estado == "Activo" ? 1 : 0);

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
        public static RespuestaAPI ActualizarProducto(ProductoNuevo producto)
        {
            var respuestaAPI = new RespuestaAPI();

            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarProducto", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", producto.Id);
            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre.Trim());
            cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion.Trim());
            cmd.Parameters.AddWithValue("@Categoria", producto.IdCategoria);
            cmd.Parameters.AddWithValue("@Precio", producto.Precio);
            cmd.Parameters.AddWithValue("@Stock", producto.Stock);
            cmd.Parameters.AddWithValue("@Proveedor", producto.IdProveedor);
            cmd.Parameters.AddWithValue("@UrlImagen", producto.UrlImagen.Trim());
            cmd.Parameters.AddWithValue("@Estado", producto.Estado == "Activo" ? 1 : 0);

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
            SqlCommand cmd = new SqlCommand("ObtenerProductos", conx);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            SqlParameter mensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultado);
            cmd.Parameters.Add(mensaje);

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
                    productos.Add(new Producto
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        IdCategoria = reader.GetInt32(reader.GetOrdinal("IdCategoria")),
                        CategoriaNombre = reader.GetString(reader.GetOrdinal("CategoriaNombre")),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                        Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                        IdProveedor = reader.GetInt32(reader.GetOrdinal("IdProveedor")),
                        ProveedorNombre = reader.GetString(reader.GetOrdinal("ProveedorNombre")),
                        UrlImagen = reader.GetString(reader.GetOrdinal("UrlImagen")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                        Estado = reader.GetBoolean(reader.GetOrdinal("Estado")) ? "Activo" : "Inactivo"
                    });
                }

                reader.Close();
                respuestaAPI.Resultado = (bool)resultado.Value;
                respuestaAPI.Mensaje = mensaje.Value?.ToString();
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
