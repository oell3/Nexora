using Nexora.Data;
using Nexora.Models;
using Nexora.utils;

namespace Nexora.Services
{
    public class ProjekteServices
    {
        private readonly Database _db;

        private readonly Helpers _helpers;

        private readonly User kunde = new();

        public ProjekteServices(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<IResult> GetAllProjekte(int mandant_id,
            int projekt_id
          )
        {

            var kunden = new KundenRepository(_db, _helpers);
            var angebote = new AngeboteRepository(_db, _helpers);
            var objekte = new ObjekteRepository(_db, _helpers);
            var rechnungen = new RechnungenRepository(_db, _helpers);
            var projekte = new ProjekteRepository(_db, _helpers);
            var termine = new TermineRepository(_db, _helpers);

            using var conn = _db.CreateConnection();

            var kundeTask = kunden.GetKundenAsync(projekt_id, mandant_id);
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
    }


}


