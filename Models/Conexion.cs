using Microsoft.Data.SqlClient;

namespace Courier.Models
{
    public class Conexion
    {
        public static SqlConnection GetConexion()
        {
            try
            {
                string cadena = "Data Source=DESKTOP-G4NOF27\\SQLEXPRESS;Initial Catalog=courier;Integrated Security=True;TrustServerCertificate=True;";
                SqlConnection cnn = new SqlConnection(cadena);
                return cnn;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
