using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Nexora.cookie;
using Nexora.Data;
using Nexora.Models;
using Npgsql;
using System.Net;
using UAParser;

namespace Nexora.Services
{
    public class UserServices(Database db, RefreshCookie refreshCookie)
    {
        private readonly PasswordHasher<object> _hasher = new();

       

        public IResult Create(string mail, string password)
        {
            using var conn = db.CreateConnection();
            var hashPassword = _hasher.HashPassword(mail, password);

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

        public async Task<IResult> Login(string mail, string password, HttpContext httpContext)
        {
            using var conn = db.CreateConnection();
            using var cmd = new NpgsqlCommand(
                "SELECT id, password_hash FROM crm.users WHERE email = @mail",
                conn
            );
            cmd.Parameters.AddWithValue("mail", mail);
            await conn.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read())
            {
                return Results.NotFound("User not found");
            }
            var storedHash = reader.GetString(1);
            var verificationResult = _hasher.VerifyHashedPassword(mail, storedHash, password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Results.Unauthorized();
            }
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            var ipString = httpContext.Connection.RemoteIpAddress?.ToString()
            ?? httpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',')[0].Trim();

            IPAddress? ip = IPAddress.TryParse(ipString, out var parsedIp) ? parsedIp : null;
            
            var deviceName = RefreshCookie.ParseDeviceName(userAgent);

            var refreshToken = refreshCookie.GenerateNewRefreshToken(db, reader.GetInt32(0), ip, deviceName);

            httpContext.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
                {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30) // je nach Token-Lebensdauer
                });


            return Results.Ok("Login successful");
        }

        public IResult Logout(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("access_token");
            httpContext.Response.Cookies.Delete("refresh_token");
            return Results.Ok();
        }
    }
}