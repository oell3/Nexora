using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Nexora.cookie;
using Nexora.cookie.Nexora.Cookie;
using Nexora.Data;
using Nexora.Models;
using Npgsql;
using System.Net;
using UAParser;

namespace Nexora.Services
{
    public class UserServices(Database db, RefreshCookie refreshCookie, JwtService jwtService)
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

        public async Task<IResult> Login(string mail, string password, HttpContext httpContext, UserRepository userRepository)
        {
            using var conn = db.CreateConnection();
            await using var cmd = new NpgsqlCommand(
                "SELECT id, password_hash FROM crm.users WHERE email = @mail", conn);
            cmd.Parameters.AddWithValue("mail", mail);
            await conn.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return Results.Unauthorized();
            }

            var storedHash = reader.GetString(1);
            if (_hasher.VerifyHashedPassword(mail, storedHash, password) == PasswordVerificationResult.Failed)
            {
                return Results.Unauthorized();
            }

            var userId = reader.GetInt32(0);
            var ip = GetClientIp(httpContext);
            if (ip is null)
            {
                return Results.BadRequest("Could not determine client IP");
            }

            var deviceName = RefreshCookie.ParseDeviceName(
                httpContext.Request.Headers["User-Agent"].ToString());
            var refreshToken = await refreshCookie.GenerateNewRefreshToken(db, userId, ip, deviceName);
            var user = await userRepository.GetUserIdbytoken(refreshToken);
            if (user is null || string.IsNullOrWhiteSpace(user.Rolle))
            {
                return Results.Problem("User data could not be loaded");
            }

            AppendAuthenticationCookies(httpContext, refreshToken,
                jwtService.CreateJwtToken(user.Id, user.Rolle, user.Mandant_id));
            return Results.Ok("Login successful");
        }

        public async Task<IResult> Refresh(HttpContext httpContext, UserRepository userRepository)
        {
            var oldRefreshToken = httpContext.Request.Cookies["refresh_token"];
            if (string.IsNullOrWhiteSpace(oldRefreshToken))
            {
                return Results.Unauthorized();
            }

            var user = await userRepository.GetUserIdbytoken(oldRefreshToken);
            if (user is null || string.IsNullOrWhiteSpace(user.Rolle))
            {
                return Results.Unauthorized();
            }

            var ip = GetClientIp(httpContext);
            if (ip is null)
            {
                return Results.BadRequest("Could not determine client IP");
            }

            var deviceName = RefreshCookie.ParseDeviceName(
                httpContext.Request.Headers["User-Agent"].ToString());
            var newRefreshToken = await refreshCookie.Refresh(db, oldRefreshToken, ip, deviceName);
            if (string.IsNullOrEmpty(newRefreshToken))
            {
                return Results.Unauthorized();
            }

            AppendAuthenticationCookies(httpContext, newRefreshToken,
                jwtService.CreateJwtToken(user.Id, user.Rolle, user.Mandant_id));
            return Results.Ok("Refresh erfolgreich!");
        }

        public IResult Logout(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("access_token", new CookieOptions { Path = "/" });
            httpContext.Response.Cookies.Delete("refresh_token", new CookieOptions { Path = "/" });
            return Results.Ok();
        }

        private static IPAddress? GetClientIp(HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress;
        }

        private static void AppendAuthenticationCookies(HttpContext httpContext, string refreshToken, string accessToken)
        {
            var isSecure = httpContext.Request.IsHttps;
            var sameSite = isSecure ? SameSiteMode.None : SameSiteMode.Lax; // browsers require Secure when SameSite=None

            httpContext.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = isSecure,
                SameSite = sameSite,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            httpContext.Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = isSecure,
                SameSite = sameSite,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });
        }
    }
}