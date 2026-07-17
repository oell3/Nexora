using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class UserRepository(Database db, Helpers helpers)
    {
        
        public async Task<List<User>> GetUsersAsync(int mandant_id)
        {
            var userList = new List<User>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.users WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var user = new User
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id"),
                    Email = helpers.GetNullableString(reader, "email"),
                    Password = helpers.GetNullableString(reader, "password"),
                    Vorname = helpers.GetNullableString(reader, "vorname"),
                    Nachname = helpers.GetNullableString(reader, "nachname"),
                    Rolle = helpers.GetNullableString(reader, "rolle"),
                    Is_owner = helpers.GetNullableBoolean(reader, "is_owner"),
                    Status = helpers.GetNullableString(reader, "status")
                };
                userList.Add(user);
            }
            return userList;
        }
    }
}
