using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotstextRevisionenRepository
    {

        private readonly Database _db;
        private readonly Helpers _helpers;

        public AngebotstextRevisionenRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<AngebotstextRevision>> GetAngebotstextRevisionenAsync(int mandant_id)
        {
            var angebotstextRevisionen = new List<AngebotstextRevision>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebotstext_revisionen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotstextRevision = new AngebotstextRevision
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Angebot_id = _helpers.GetNullableInt(reader, "angebotstext_id"),
                    Revisionsnummer = _helpers.GetNullableInt(reader, "revisionsnummer"),
                    Erstellt_am = _helpers.GetNullableDateTime(reader, "erstellt_am"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotstextRevisionen.Add(angebotstextRevision);
            }
            return angebotstextRevisionen;
        }
    }
}
