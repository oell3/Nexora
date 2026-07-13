using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotRevisionRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;

        public AngebotRevisionRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<AngebotRevision>> GetAngebotRevisionenAsync(int mandant_id)
        {
            var angebotRevisionen = new List<AngebotRevision>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebot_revisionen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotRevision = new AngebotRevision
                {
                    Id = _helpers.GetNullableInt(reader, "id") ?? 0,
                    Angebot_id = _helpers.GetNullableInt(reader, "angebot_id"),
                    Revisionsnummer = _helpers.GetNullableInt(reader, "revisionsnummer"),
                    Erstellt_am = _helpers.GetNullableDateTime(reader, "datum"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotRevisionen.Add(angebotRevision);
            }
            return angebotRevisionen;
        }

    }
}
