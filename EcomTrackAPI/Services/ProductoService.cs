using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;
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
            conx.Open();
            SqlDataReader reader = command.ExecuteReader();
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
            reader.Close();
            command.Dispose();
            conx.Close();
            return productos;
        }
    }
}