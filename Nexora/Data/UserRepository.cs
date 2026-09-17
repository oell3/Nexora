using Microsoft.IdentityModel.Tokens;
using Nexora.Models;
using Nexora.utils;
using Nexora.cookie;
using System.Data;

namespace Nexora.Data
{
    public class UserRepository(Database db, Helpers helpers, RefreshCookie refreshCookie)
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
                  
                    Id = helpers.GetInt(reader, "id"),
                    Mandant_id = helpers.GetInt(reader, "mandant_id"),
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

        

        public async Task<User?> GetUserIdbytoken(string refreshtoken)
        {   //userid, mandantid, role
            using var conn = db.CreateConnection();
            
            await conn.OpenAsync();
            
            using var cmd = new Npgsql.NpgsqlCommand(
                "SELECT u.id, u.mandant_id, u.rolle FROM crm.users u " +
                "JOIN crm.refresh_tokens r ON u.id = r.user_id " +
                "WHERE r.token_hash = @token_hash AND r.revoked = false AND r.expires_at > NOW()",
                conn);
            
            var token_hash = refreshCookie.HashToken(refreshtoken);
            
            cmd.Parameters.AddWithValue("token_hash", token_hash);

            await using var reader = await cmd.ExecuteReaderAsync();
           
            if (await reader.ReadAsync()) { 
                var user = new User
                {
                    Id = reader.GetInt32(0),
                    Mandant_id = reader.GetInt32(1),
                    Rolle = reader.GetString(2)
                };

                return user;
            }

            return null;
        }
    }
}
