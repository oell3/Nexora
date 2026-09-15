using Npgsql;

namespace Nexora.Data
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(IConfiguration config)
        {
            var section = config.GetSection("Database");

            var host = section["Host"];
            var port = section["Port"];
            var user = section["Username"];
            var pass = section["Password"];
            var db = section["Database"];
            

            var portPart = string.IsNullOrWhiteSpace(port) ? string.Empty : $"Port={port};";

            _connectionString =
                $"Host={host};{portPart}Username={user};Password={pass};Database={db};Maximum Pool Size=20;";
        }


        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}