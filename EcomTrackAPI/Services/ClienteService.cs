using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
namespace EcomTrackAPI.Services
{
    public class ClienteService
    {
        public static List<Cliente> ObtenerClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            SqlConnection conx = Datos.conectar();

            string query = "SELECT Id, Nombre, Correo, Pwd, Telefono, FechaRegistro FROM Cliente";
            SqlCommand command = new SqlCommand(query, conx);

            conx.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Cliente cliente = new Cliente
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Correo = reader.GetString(2),
                    Pwd = reader.GetString(3),
                    Telefono = reader.GetString(4),
                    FechaRegistro = reader.GetDateTime(5)
                };
                clientes.Add(cliente);
            }

            reader.Close();
            command.Dispose();
            conx.Close();

            return clientes;
        }
    }
}
