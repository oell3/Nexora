using Nexora.cookie;
using Nexora.Models;
using Nexora.Services;
using UAParser;

namespace Nexora.API
{
    public static class UserEndpoint
    {



        public static void MapUserEndpoint(this RouteGroupBuilder api)
        {


            var _group = api.MapGroup("/user");

            _group.MapPost("/", (UserServices service, User user) =>
            {
                if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                {
                    return Results.BadRequest("Kunde object is null or missing required properties.");
                }
                return service.Create(user.Email, user.Password);
            });

            _group.MapPost("/login", async (UserServices service, User user, HttpContext httpContext) =>
            {
                Console.Write(user);
                if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                {
                    return Results.BadRequest("Kunde object is null or missing required properties.");
                }
             
                return await service.Login(user.Email, user.Password, httpContext);
            });

            _group.MapGet("/logout", (UserServices service, HttpContext httpcontext) =>
            {
                return service.Logout(httpcontext);
            });
        }

    }
}
