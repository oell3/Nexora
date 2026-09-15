using Nexora.Services;

namespace Nexora.API
{
    public static class AuthEndpoint
    {
        public static void MapAuthEndpoint(this RouteGroupBuilder api)
        {
            var _group = api.MapGroup("/auth");
            _group.MapGet("/status", () =>
            {
                return Results.Json(new { authenticated = true });
            });
        }
    }
}
