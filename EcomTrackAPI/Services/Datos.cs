using Microsoft.Data.SqlClient;
namespace EcomTrackAPI.Services
{
    public class Datos
    {
        public static SqlConnection conectar()
        {
            SqlConnection conx = new SqlConnection();
            conx.ConnectionString = "Server=tcp:alydev.database.windows.net;Initial Catalog=DB_EcomTrack;User ID=alydev;Password=Tilines123;";
            return conx;
        }
    }
}
