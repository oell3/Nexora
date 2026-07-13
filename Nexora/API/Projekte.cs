using Nexora.Services;

namespace Nexora.API
{
    public static class ProjekteApi
    {

        public static void MapProjekteEndpoint(this RouteGroupBuilder api)
        {
            var _group = api.MapGroup("/projekte");
            _group.MapGet("/", async (ProjekteServices services) =>
            {

                Console.Write(await services.GetAllProjekte(1, 1));
                return await services.GetAllProjekte(1, 1);
            });
        }
    }
}
