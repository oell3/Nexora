using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotstextRevisionenRepository(Database db, Helpers helpers)
    {

        public async Task<List<AngebotstextRevision>> GetAngebotstextRevisionenAsync(int mandant_id)
        {
            var angebotstextRevisionen = new List<AngebotstextRevision>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebotstext_revisionen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotstextRevision = new AngebotstextRevision
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Angebot_id = helpers.GetNullableInt(reader, "angebotstext_id"),
                    Revisionsnummer = helpers.GetNullableInt(reader, "revisionsnummer"),
                    Erstellt_am = helpers.GetNullableDateTime(reader, "erstellt_am"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotstextRevisionen.Add(angebotstextRevision);
            }
            return angebotstextRevisionen;
        }
    }
}
