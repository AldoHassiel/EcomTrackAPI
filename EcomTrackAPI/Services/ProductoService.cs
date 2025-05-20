using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
namespace EcomTrackAPI.Services
{
    public class ProductoService
    {
        public static List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();
            SqlConnection conx = Datos.conectar();
            string query = "SELECT Id, Nombre, Descripcion, Categoria, Precio, Stock, Proveedor FROM Producto";
            SqlCommand command = new SqlCommand(query, conx);
            SqlDataReader reader = null;
            try
            {
                conx.Open();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Producto producto = new Producto
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        IdCategoria = reader.GetInt32(3),
                        Precio = reader.GetDecimal(4),
                        Stock = reader.GetInt32(5),
                        IdProveedor = reader.GetInt32(6)
                    };
                    productos.Add(producto);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener los productos: " + ex.Message, ex);
            }
            finally
            {
                reader?.Close();
                command.Dispose();
                conx.Close();
            }

            return productos;
        }
        public static Producto ObtenerProductoPorId(int id)
        {
            Producto producto = null;
            SqlConnection conx = Datos.conectar();
            string query = "SELECT Id, Nombre, Descripcion, Categoria, Precio, Stock, Proveedor FROM Producto WHERE Id = @Id";
            SqlCommand command = new SqlCommand(query, conx);
            command.Parameters.AddWithValue("@Id", id);
            SqlDataReader reader = null;
            try
            {
                conx.Open();
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    producto = new Producto
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        IdCategoria = reader.GetInt32(3),
                        Precio = reader.GetDecimal(4),
                        Stock = reader.GetInt32(5),
                        IdProveedor = reader.GetInt32(6)
                    };
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener el producto: " + ex.Message, ex);
            }
            finally
            {
                reader?.Close();
                command.Dispose();
                conx.Close();
            }
            return producto;
        }
        public static string GuardarProducto(Producto producto)
        {
            string mensaje;
            SqlConnection conx = Datos.conectar();
            SqlCommand command = new SqlCommand("GuardarProducto", conx);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", producto.Id);
            command.Parameters.AddWithValue("@Nombre", producto.Nombre);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Categoria", producto.IdCategoria);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.Parameters.AddWithValue("@Stock", producto.Stock);
            command.Parameters.AddWithValue("@Proveedor", producto.IdProveedor);

            try
            {
                conx.Open();
                command.ExecuteNonQuery();
                mensaje = "Ok";
            }
            catch (SqlException ex)
            {
                mensaje = "Error al guardar el producto: " + ex.Message;
            }
            finally
            {
                command.Dispose();
                conx.Close();
            }

            return mensaje;
        }
        public static string EliminarProducto(int id)
        {
            string mensaje;
            SqlConnection conx = Datos.conectar();
            string query = "DELETE FROM Producto WHERE Id = @Id";
            SqlCommand command = new SqlCommand(query, conx);
            command.Parameters.AddWithValue("@Id", id);
            try
            {
                conx.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    mensaje = "Ok";
                }
                else
                {
                    mensaje = "No se encontró el producto con el Id especificado.";
                }
            }
            catch (SqlException ex)
            {
                mensaje = "Error al eliminar el producto: " + ex.Message;
            }
            finally
            {
                command.Dispose();
                conx.Close();
            }
            return mensaje;
        }
    }
}