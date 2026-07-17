using Nexora.Models;
using Nexora.utils;
namespace Nexora.Data
{
    public class VertraegeRepository(Database db, Helpers helpers)
    {
        

        public async Task<List<Vertraeg>> GetVertraegeAsync(int mandant_id)
        {
            var vertraege = new List<Vertraeg>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.vertraege WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var vertrag = new Vertraeg
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Kunden_id = helpers.GetNullableInt(reader, "kunden_id"),
                    Objekt_id = helpers.GetNullableInt(reader, "projekt_id"),
                    Pfad = helpers.GetNullableString(reader, "pfad"),
                    Art = helpers.GetNullableString(reader, "art"),
                    Datum = helpers.GetNullableDateTime(reader, "datum"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                    vertraege.Add(vertrag);
            }
            return vertraege;
        }

    }
}
