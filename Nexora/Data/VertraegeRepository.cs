using Nexora.Models;
using Nexora.utils;
namespace Nexora.Data
{
    public class VertraegeRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;

        public VertraegeRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }



        public async Task<List<Vertraeg>> GetVertraegeAsync(int mandant_id)
        {
            var vertraege = new List<Vertraeg>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.vertraege WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var vertrag = new Vertraeg
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Kunden_id = _helpers.GetNullableInt(reader, "kunden_id"),
                    Objekt_id = _helpers.GetNullableInt(reader, "projekt_id"),
                    Pfad = _helpers.GetNullableString(reader, "pfad"),
                    Art = _helpers.GetNullableString(reader, "art"),
                    Datum = _helpers.GetNullableDateTime(reader, "datum"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                    vertraege.Add(vertrag);
            }
            return vertraege;
        }

    }
}
