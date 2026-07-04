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
            var user = section["Username"];
            var pass = section["Password"];
            var db = section["Database"];

            _connectionString =
                $"Host={host};Username={user};Password={pass};Database={db}";
        }


        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}