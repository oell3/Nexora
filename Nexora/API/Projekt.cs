using Nexora.Services;

namespace Nexora.API
{
    public static class ProjektApi
    {

        public static void MapProjekteEndpoint(this RouteGroupBuilder api)
        {
            var _group = api.MapGroup("/projekte");
            _group.MapGet("/", async (ProjekteServices services) =>
            {

                Console.Write(await services.GetAllProjekte(1, 1));
                return await services.GetAllProjekte(1, 1);
            });

            _group.MapGet("/angebot", async (int mandant_id, ProjekteServices services) =>
            {
                
                return await services.GetAllProjektbyMandant(mandant_id);
            });
        }
    }
}
