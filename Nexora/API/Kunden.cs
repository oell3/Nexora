using Nexora.Data;
using Nexora.Models;

namespace Nexora.API
{
        public static class KundenEndPoint
        {
            public static void MapKundenEndPoint(this RouteGroupBuilder api)
            {
                var group = api.MapGroup("/kunden");

                group.MapGet("/", async (KundenRepository kundenRepository, HttpContext httpContext) =>
                {
                    if (httpContext.User.Identity?.IsAuthenticated != true)
                    {
                        return Results.Unauthorized();
                    }

                    var mandantIdClaim = httpContext.User.Claims.FirstOrDefault(claim =>
                        string.Equals(claim.Type, "Mandant_id", StringComparison.OrdinalIgnoreCase));

                    if (mandantIdClaim is null || string.IsNullOrWhiteSpace(mandantIdClaim.Value))
                    {
                        return Results.BadRequest(new
                        {
                            Fehler = "Der Mandant_id-Claim ist im JWT nicht vorhanden."
                        });
                    }

                    if (!int.TryParse(mandantIdClaim.Value, out var mandantId))
                    {
                        return Results.BadRequest(new
                        {
                            Fehler = "Der Mandant_id-Claim enthält keinen gültigen Wert."
                        });
                    }

                    var kunden = await kundenRepository.GetKundenAsync(mandantId);

                    return Results.Json(kunden);
                });

            group.MapGet("/angebot", async (KundenRepository kundenRepository, HttpContext httpContext) =>
            {
               /* if (httpContext.User.Identity?.IsAuthenticated != true)
                {
                    return Results.Unauthorized();
                }

                var mandantIdClaim = httpContext.User.Claims.FirstOrDefault(claim =>
                    string.Equals(claim.Type, "Mandant_id", StringComparison.OrdinalIgnoreCase));

                if (mandantIdClaim is null || string.IsNullOrWhiteSpace(mandantIdClaim.Value))
                {
                    return Results.BadRequest(new
                    {
                        Fehler = "Der Mandant_id-Claim ist im JWT nicht vorhanden."
                    });
                }

                if (!int.TryParse(mandantIdClaim.Value, out var mandantId))
                {
                    return Results.BadRequest(new
                    {
                        Fehler = "Der Mandant_id-Claim enthält keinen gültigen Wert."
                    });
                }*/

                var kunden = await kundenRepository.GetKundenAsync(1);

                return Results.Json(kunden);
            });
        }
        }       
}