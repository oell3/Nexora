using Nexora.Models;
using Nexora.Data;
using System.Runtime.CompilerServices;

namespace Nexora.API
{
    public static class AngebotEndPoint
    {
        public static void MapAngebotEndPoint(this RouteGroupBuilder api)
        {
            var _group = api.MapGroup("/angebot");

            _group.MapPost("/save", async (Angebot angebot, AngebotRepository angebotRepository, HttpContext httpContext) =>
            {
                /*var mandantClaim = httpContext.User.FindFirst("mandantId")?.Value;
                var userClaim = httpContext.User.FindFirst("userId")?.Value;

                if (!int.TryParse(mandantClaim, out var mandant_id) || !int.TryParse(userClaim, out var user_id))
                {
                    return Results.Unauthorized();
                }
                */
                Console.WriteLine("Hallo");

                Console.WriteLine(angebot.AngebotGruppe);
                foreach(var gruppe in angebot.AngebotGruppe)
                {
                    Console.WriteLine(gruppe);
                    
                }
                var angebot_id = await angebotRepository.InsertAngebot(angebot, 1, 1);



                return Results.Ok(new { id = angebot_id });
            });
        }
    }
}
