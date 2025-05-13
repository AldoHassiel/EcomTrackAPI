using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.PortableExecutable;
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
        public static Cliente ObtenerCliente(int Id)
        {
            Cliente cliente = new Cliente();
            SqlConnection conx = Datos.conectar();

            string query = $"SELECT Nombre, Correo, Pwd, Telefono, FechaRegistro FROM Cliente WHERE Id = {Id}";
            SqlCommand command = new SqlCommand(query, conx);

            conx.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                cliente.Id = Id;
                cliente.Nombre = reader.GetString(0);
                cliente.Correo = reader.GetString(1);
                cliente.Pwd = reader.GetString(2);
                cliente.Telefono = reader.GetString(3);
                cliente.FechaRegistro = reader.GetDateTime(4);
            }

            reader.Close();
            command.Dispose();
            conx.Close();

            return cliente;
        }
        public static string GuardarCliente(Cliente cliente)
        {
            string mensaje = "Ok";

            SqlConnection conx = Datos.conectar();

            SqlCommand command = new SqlCommand("GuardarCliente", conx);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", cliente.Id);
            command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@Correo", cliente.Correo);
            command.Parameters.AddWithValue("@Pwd", cliente.Pwd);
            command.Parameters.AddWithValue("@Telefono", cliente.Telefono);

            conx.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }
            finally
            {
                command.Dispose();
            }

            conx.Close();
            return mensaje;
        }
    }
}
