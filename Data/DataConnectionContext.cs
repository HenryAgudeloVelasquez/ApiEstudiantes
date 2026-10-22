using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ApiEstudiantes.Data
{
    public class DataConnectionContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DataConnectionContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("connectiondb")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'connectiondb' en la configuración.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
