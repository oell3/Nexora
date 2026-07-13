using Nexora.utils;
using Nexora.Models;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotstextRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;

        private AngebotstextRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<Angebotstext>> GetAngebotstexteAsync(int mandant_id)
        {
            var angebotstexte = new List<Angebotstext>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebotstext WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotstext = new Angebotstext
                {
                    Id = _helpers.GetNullableInt(reader, "id") ?? 0,
                    Angebot_id = _helpers.GetNullableInt(reader, "angebot_id"),
                    Typ = _helpers.GetNullableInt(reader, "bezeichnung"),
                    Text = _helpers.GetNullableString(reader, "text"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotstexte.Add(angebotstext);
            }
            return angebotstexte;
        }
    }
}
