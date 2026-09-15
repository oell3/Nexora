using Microsoft.IdentityModel.Tokens;
using Nexora.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Nexora.API
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Middleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        public Middleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value?.ToLower();

            var publicPaths = new[]
            {
        "/api/user/login",
        "/api/user/refresh",
        "/api/angebot/save"
            };

            if (publicPaths.Contains(path))
            {
                await _next(httpContext);
                return;
            }
            
            var token = httpContext.Request.Cookies["access_token"]; // Cookie-Name anpassen
            Console.Write("token: " + token);
            if (string.IsNullOrEmpty(token))
            {
                httpContext.Response.StatusCode = 401;
                Console.WriteLine("Raw Cookie Header: " + httpContext.Request.Headers["Cookie"]);
                await httpContext.Response.WriteAsync("Kein Token vorhanden");
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!); // Secret aus appsettings/Env

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                httpContext.User = principal; // Claims (Sub, Role, mandant_id) verfügbar machen

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync(ex.Message);
            }
        }
    }
    

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
}
