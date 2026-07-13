using Microsoft.AspNetCore.Identity;
using Nexora.Data;
using Npgsql;

namespace Nexora.Services
{
    public class UsersServices
    {
        private readonly Database _db;
        private readonly PasswordHasher<object> _hasher = new();

        public UsersServices(Database db)
        {
            _db = db;
        }

        public IResult Create(string mail, string password)
        {
            using var conn = _db.CreateConnection();
            var hashPassword = _hasher.HashPassword(null, password);

            using var cmd = new NpgsqlCommand(
                "Insert Into crm.users (email, password_hash, mandant_id, rolle) " +
                "VALUES ( @mail, @password, '1', 'user')",
                conn
            );



            cmd.Parameters.AddWithValue("mail", mail);
            cmd.Parameters.AddWithValue("password", hashPassword);
            conn.Open();
            int i = cmd.ExecuteNonQuery();

            if (i == 0)
            {
                return Results.NotFound();
            }
            return Results.Ok();
        }

    }
}