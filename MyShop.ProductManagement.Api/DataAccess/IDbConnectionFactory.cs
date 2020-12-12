using System.Data;
using System.Data.SqlClient;

namespace MyShop.ProductManagement.Api.DataAccess
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection(string connectionString);
    }

    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}