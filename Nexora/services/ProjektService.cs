using Nexora.Data;
using Nexora.Models;
using Nexora.utils;

namespace Nexora.Services
{
    public class ProjekteServices(Database db, Helpers helpers)
    {


        public async Task<IResult> GetAllProjekte(int mandant_id,
            int projekt_id
          )
        {

            var kunden = new KundenRepository(db, helpers);
            var angebote = new AngeboteRepository(db, helpers);
            var objekte = new ObjekteRepository(db, helpers);
            var rechnungen = new RechnungenRepository(db, helpers);
            var projekte = new ProjektRepository(db, helpers);
            var termine = new TermineRepository(db, helpers);

            using var conn = db.CreateConnection();

            var kundeTask = kunden.GetKundenAsync(mandant_id);
            var angeboteTask = angebote.GetAngeboteProjekteAsync(projekt_id, mandant_id);
            var objekteTask = objekte.GetObjekteAsync(mandant_id);
            var rechnungenTask = rechnungen.GetRechnungenProjekteAsync(projekt_id, mandant_id);
            var projekteTask = projekte.GetProjekteAsync(projekt_id, mandant_id);
            var termineTask = termine.GetTermineAsync(mandant_id);
            //  var vertraegeTask = GetVertraegeAsync(projekt_id, mandant_id);


            await Task.WhenAll(
                kundeTask,
                angeboteTask,
                objekteTask,
                 rechnungenTask,
                 projekteTask,
              termineTask
            //vertraegeTask
            );


            var kundeRes = await kundeTask;
            var angeboteRes = await angeboteTask;
            var objekteRes = await objekteTask;
            var rechnungenRes = await rechnungenTask;
            var projekteRes = await projekteTask;
            var termineRes = await termineTask;
            //var vertraegeRes = await vertraegeTask;

            return Results.Json(new
            {
                kundeRes,
                angeboteRes,
                objekteRes,
                rechnungenRes,
                projekteRes,
                termineRes
            });
        }


        public async Task<IResult> GetAllProjektbyMandant(int mandant_id)
        {
            var projekt = new ProjektRepository(db, helpers);

            var projektRes = await projekt.GetAllProjekbyMandantAsync(mandant_id);

            return Results.Json(projektRes);

        }

    }
}


