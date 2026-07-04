namespace Nexora.API
{
    public class Projekte
    {   

        
        public static void MapProjekteEndpoint(this RouteGroupBuilder api)
        {
            var _group = api.MapGroup("/projekte");
            _group.MapGet("/", () =>
            {
                return Results.Ok("Projekte Endpoint");
            });
        }
    }
}
