using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class UserRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;
        public UserRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }
        public async Task<List<User>> GetUsersAsync(int mandant_id)
        {
            var userList = new List<User>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.users WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var user = new User
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id"),
                    Email = _helpers.GetNullableString(reader, "email"),
                    Password = _helpers.GetNullableString(reader, "password"),
                    Vorname = _helpers.GetNullableString(reader, "vorname"),
                    Nachname = _helpers.GetNullableString(reader, "nachname"),
                    Rolle = _helpers.GetNullableString(reader, "rolle"),
                    Is_owner = _helpers.GetNullableBoolean(reader, "is_owner"),
                    Status = _helpers.GetNullableString(reader, "status")
                };
                userList.Add(user);
            }
            return userList;
        }
    }
}
