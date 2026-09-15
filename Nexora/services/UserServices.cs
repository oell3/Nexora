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
            using var cmd = new NpgsqlCommand(
                "SELECT id, password_hash FROM crm.users WHERE email = @mail",
                conn
            );
            cmd.Parameters.AddWithValue("mail", mail);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return Results.Unauthorized();
            }

            var storedHash = reader.GetString(1);
            var verificationResult = _hasher.VerifyHashedPassword(mail, storedHash, password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Results.Unauthorized();
            }

            var userId = reader.GetInt32(0);

            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            var ipString = httpContext.Connection.RemoteIpAddress?.ToString()
                ?? httpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',')[0].Trim();

            if (!IPAddress.TryParse(ipString, out var ip))
            {
                return Results.BadRequest("Could not determine client IP");
            }

            var deviceName = RefreshCookie.ParseDeviceName(userAgent);

            var refreshToken = await refreshCookie.GenerateNewRefreshToken(db, userId, ip, deviceName);

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Results.Problem("Failed to generate refresh token");
            }

            var user = await userRepository.GetUserIdbytoken(refreshToken);

            if (user is null || user.Rolle == null)
            {
                throw new Exception("Kein User gefunden!");
            }

            string jwtCookie = jwtService.CreateJwtToken(user.Id, user.Rolle, user.Mandant_id);

            httpContext.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            httpContext.Response.Cookies.Append("access_token", jwtCookie, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

            return Results.Ok("Login successful");
        }

        public async Task<IResult> Refresh(HttpContext httpContext, UserRepository userRepository)
        {

            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            var ipString = httpContext.Connection.RemoteIpAddress?.ToString()
                ?? httpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',')[0].Trim();

            if (!IPAddress.TryParse(ipString, out var ip))
            {
                return Results.BadRequest("Could not determine client IP");
            }
            var CookierefreshToken = httpContext.Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(CookierefreshToken))
            {
                return Results.Unauthorized();
            }


            var deviceName = RefreshCookie.ParseDeviceName(userAgent);

            var refreshToken = await refreshCookie.Refresh(db, CookierefreshToken, ip, deviceName);

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Results.Problem("Failed to generate refresh token");
            }

            var user = await userRepository.GetUserIdbytoken(CookierefreshToken);

            if(user is null || user.Rolle == null)
            {
                throw new Exception("Kein User gefunden!");
            }

            string jwtCookie = jwtService.CreateJwtToken(user.Id, user.Rolle, user.Mandant_id);


            httpContext.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });


            httpContext.Response.Cookies.Append("access_token", jwtCookie, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

            return Results.Ok("Refresh erfolgreich!");
        }

        public IResult Logout(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("access_token");
            httpContext.Response.Cookies.Delete("refresh_token");
            return Results.Ok();
        }
    }
}