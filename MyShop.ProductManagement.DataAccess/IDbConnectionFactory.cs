using System;
using System.Data;
using System.Data.SqlClient;

namespace MyShop.ProductManagement.DataAccess
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }

    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(DatabaseConfig config)
        {
            if (string.IsNullOrWhiteSpace(config?.ConnectionString))
            {
                throw new ArgumentNullException(nameof(DatabaseConfig.ConnectionString));
            }

            _connectionString = config.ConnectionString;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}