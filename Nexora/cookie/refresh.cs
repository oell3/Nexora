using Nexora.Data;
using Npgsql;
using NpgsqlTypes;
using System.Net;
using UAParser;
using System.Security.Cryptography;

namespace Nexora.cookie;
public class  RefreshCookie()
{

    public async Task<string> Refresh(Database db, string refreshToken, IPAddress ip, string devicename)
    {
        var tokenHash = HashToken(refreshToken);
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(
            "SELECT user_id FROM crm.refresh_tokens WHERE token_hash = @token_hash", conn
            );
        cmd.Parameters.AddWithValue("token_hash", tokenHash);
        var res = await cmd.ExecuteScalarAsync();
        int user_id = Convert.ToInt32(res);


        return await GenerateNewRefreshToken(db, user_id, ip, devicename);
    }
    public async Task<string> GenerateNewRefreshToken(Database db,int userID, IPAddress ip, string devicename)
    {
        var refreshToken = CreateRefreshToken();
        var hashedToken = HashToken(Convert.ToBase64String(refreshToken));
        
        SaveRefreshToken(db, userID, ip, devicename, hashedToken, DateTime.UtcNow.AddDays(30));
        await RevokeRefreshToken(db, userID, hashedToken);

        return Convert.ToBase64String(refreshToken);
    }

    private async static void SaveRefreshToken(Database db, int userID, IPAddress ip, string devicename,string hashedToken, DateTime expiration)
    {

        using var conn = db.CreateConnection();
        using var cmd = new NpgsqlCommand(
            "INSERT INTO crm.refresh_tokens (user_id, ip, device_name, token_hash, expires_at) " +
            "VALUES (@userID, @ip, @devicename, @hashedToken, @expiration)",
            conn
        );
        cmd.Parameters.AddWithValue("userID", userID);
        cmd.Parameters.AddWithValue("ip", NpgsqlDbType.Inet,  ip);
        cmd.Parameters.AddWithValue("devicename", devicename);
        cmd.Parameters.AddWithValue("hashedToken", hashedToken);
        cmd.Parameters.AddWithValue("expiration", expiration);
        await conn.OpenAsync();
        var res = await cmd.ExecuteNonQueryAsync();

        if(res == 0)
        {
            throw new Exception("Failed to save refresh token.");
        }
        // Save the hashed token and expiration date to your database or storage
    }
    private static async Task RevokeRefreshToken(Database db, int userId, string hashedToken)
    {
        using var conn = db.CreateConnection();
    using var cmd = new NpgsqlCommand(
        "UPDATE crm.refresh_tokens " +
        "SET revoked = true, " +
        "revoked_at = NOW(), " +
        "reason_logout = 'Token Refresh' " +
        "WHERE token_hash = @token_hash " +
        "AND revoked = false " +
        "AND user_id = @userId",
        conn);

        cmd.Parameters.AddWithValue("token_hash", hashedToken);
        cmd.Parameters.AddWithValue("userId", userId);

    await conn.OpenAsync();
    var rowsAffected = await cmd.ExecuteNonQueryAsync();

    }
    public byte[] CreateRefreshToken()
    {
        var randomNumber = new byte[64];

        return randomNumber;
    }

    public string HashToken(string token)
    {

        
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    public static string ParseDeviceName(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return "Unknown";

        var uaParser = Parser.GetDefault();
        ClientInfo c = uaParser.Parse(userAgent);

        // z.B. "Chrome 126 / Windows 10"
        return $"{c.UA.Family} {c.UA.Major} / {c.OS.Family} {c.OS.Major}";
    }


}
