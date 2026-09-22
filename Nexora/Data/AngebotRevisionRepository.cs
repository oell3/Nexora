using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotRevisionRepository(Database db, Helpers helpers)
    {

        public async Task<List<AngebotRevision>> GetAngebotRevisionenAsync(int mandant_id)
        {
            var angebotRevisionen = new List<AngebotRevision>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebot_revisionen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotRevision = new AngebotRevision
                {
                    Id = helpers.GetNullableInt(reader, "id") ?? 0,
                    Angebot_id = helpers.GetNullableInt(reader, "angebot_id"),
                    Revisionsnummer = helpers.GetNullableInt(reader, "revisionsnummer"),
                    Erstellt_am = helpers.GetNullableDateTime(reader, "datum"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotRevisionen.Add(angebotRevision);
            }
            return angebotRevisionen;
        }
        //testnssswsss
    }
}
