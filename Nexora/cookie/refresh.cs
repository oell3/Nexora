using Nexora.Data;
using Npgsql;
using NpgsqlTypes;
using System.Net;
using UAParser;
using System.Security.Cryptography;

namespace Nexora.cookie;

public sealed class RefreshCookie
{
    public async Task<string?> Refresh(Database db, string refreshToken, IPAddress ip, string deviceName)
    {
        var tokenHash = HashToken(refreshToken);
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT user_id FROM crm.refresh_tokens " +
            "WHERE token_hash = @token_hash AND revoked = false AND expires_at > NOW()",
            conn);
        cmd.Parameters.AddWithValue("token_hash", tokenHash);

        var result = await cmd.ExecuteScalarAsync();
        if (result is null || result is DBNull)
        {
            return null;
        }

        var userId = Convert.ToInt32(result);
        var newToken = await CreateAndStoreRefreshToken(db, userId, ip, deviceName);
        if (await RevokeRefreshToken(db, userId, tokenHash) != 1)
        {
            return null;
        }

        return newToken;
    }

    public async Task<string> GenerateNewRefreshToken(Database db, int userId, IPAddress ip, string deviceName)
    {
        return await CreateAndStoreRefreshToken(db, userId, ip, deviceName);
    }

    private async Task<string> CreateAndStoreRefreshToken(Database db, int userId, IPAddress ip, string deviceName)
    {
        var refreshToken = CreateRefreshToken();
        var token = Convert.ToBase64String(refreshToken);
        var tokenHash = HashToken(token);

        await SaveRefreshToken(db, userId, ip, deviceName, tokenHash, DateTime.UtcNow.AddDays(30));
        return token;
    }

    private static async Task SaveRefreshToken(
        Database db,
        int userId,
        IPAddress ip,
        string deviceName,
        string tokenHash,
        DateTime expiration)
    {
        using var conn = db.CreateConnection();
        await using var cmd = new NpgsqlCommand(
            "INSERT INTO crm.refresh_tokens (user_id, ip, device_name, token_hash, expires_at) " +
            "VALUES (@userId, @ip, @deviceName, @tokenHash, @expiration)",
            conn);
        cmd.Parameters.AddWithValue("userId", userId);
        cmd.Parameters.AddWithValue("ip", NpgsqlDbType.Inet, ip);
        cmd.Parameters.AddWithValue("deviceName", deviceName);
        cmd.Parameters.AddWithValue("tokenHash", tokenHash);
        cmd.Parameters.AddWithValue("expiration", expiration);

        await conn.OpenAsync();
        if (await cmd.ExecuteNonQueryAsync() != 1)
        {
            throw new InvalidOperationException("Failed to save refresh token.");
        }
    }

    private static async Task<int> RevokeRefreshToken(Database db, int userId, string tokenHash)
    {
        using var conn = db.CreateConnection();
        await using var cmd = new NpgsqlCommand(
            "UPDATE crm.refresh_tokens " +
            "SET revoked = true, revoked_at = NOW(), reason_logout = 'Token Refresh' " +
            "WHERE token_hash = @tokenHash AND revoked = false AND expires_at > NOW() AND user_id = @userId",
            conn);
        cmd.Parameters.AddWithValue("tokenHash", tokenHash);
        cmd.Parameters.AddWithValue("userId", userId);

        await conn.OpenAsync();
        return await cmd.ExecuteNonQueryAsync();
    }

    public static byte[] CreateRefreshToken()
    {
        var token = new byte[64];
        RandomNumberGenerator.Fill(token);
        return token;
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
