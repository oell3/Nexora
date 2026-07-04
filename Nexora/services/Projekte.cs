using Microsoft.AspNetCore.Http.HttpResults;
using Nexora.Data;
using Nexora.Models;
using Npgsql;
using System.Threading.Tasks;

namespace Nexora.services
{
    public class Projekte
    {
        private readonly Database _db;

        private readonly User kunde = new();
        public Projekte(Database db) {
            _db = db;
        }

        public async Task<IResult> GetAllProjekte(int mandant_id)
        {

            using var conn = _db.CreateConnection();


            var kundeTask = GetKundenAsync(mandant_id);
            var angeboteTask = GetAngeboteAsync(mandant_id);
            var objekteTask = GetObjekteAsync(mandant_id);
            var rechnungenTask = GetRechnungenAsync(mandant_id);
            var projekteTask = GetProjekteAsync(mandant_id);
            var termineTask = GetTermineAsync(mandant_id);
            var vertraegeTask = GetVertraegeAsync(mandant_id);


            await Task.WhenAll(
                kundeTask,
                angeboteTask,
                objekteTask,
                rechnungenTask,
                projekteTask,
                termineTask,
                vertraegeTask
            );


            var kundeRes = await kundeTask;
            var angeboteRes = await angeboteTask;
            var objekteRes = await objekteTask;
            var rechnungenRes = await rechnungenTask;
            var projekteRes = await projekteTask;
            var termineRes = await termineTask;
            var vertraegeRes = await vertraegeTask;

            return Results.Ok();
        }
    }

    private async Task<List<User>> GetKundenAsync(int id)
        {
            using var conn = _db.CreateConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.kunden WHERE mandant_id = @id", conn);
            conn.Open();
            cmd.Parameters.AddWithValue("id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            var kunden = new List<User>();
            while (await reader.ReadAsync())
            {
                var kunde = new User
                {
                    ewmail = reader.GetString(0),
                    paswsword = reader.GetString(1),
                    // Map other properties as needed
                };
                kunden.Add(kunde);
            }
            return kunden;
        }
    } 
}
