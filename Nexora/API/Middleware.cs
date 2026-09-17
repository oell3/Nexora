using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Nexora.API;

public sealed class Middleware(RequestDelegate next, IConfiguration configuration)
{
    private static readonly HashSet<string> PublicPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/api/user",
        "/api/user/",
        "/api/user/login",
        "/api/user/refresh"
    };

    public async Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsOptions(context.Request.Method) ||
            PublicPaths.Contains(context.Request.Path.Value ?? string.Empty))
        {
            await next(context);
            return;
        }

        var token = context.Request.Cookies["access_token"];
        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authentication required");
            return;
        }

        var secret = configuration["Jwt:Secret"];
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException("JWT secret not configured");
        }

        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            context.User = principal;
            await next(context);
        }
        catch (SecurityTokenException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid or expired access token");
        }
    }
}

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseNexoraAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Middleware>();
    }
}
