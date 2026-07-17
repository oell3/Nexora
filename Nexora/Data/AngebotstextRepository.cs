using Nexora.utils;
using Nexora.Models;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotstextRepository(Database db, Helpers helpers)
    {
  

        public async Task<List<Angebotstext>> GetAngebotstexteAsync(int mandant_id)
        {
            var angebotstexte = new List<Angebotstext>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebotstext WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotstext = new Angebotstext
                {
                    Id = helpers.GetNullableInt(reader, "id") ?? 0,
                    Angebot_id = helpers.GetNullableInt(reader, "angebot_id"),
                    Typ = helpers.GetNullableInt(reader, "bezeichnung"),
                    Text = helpers.GetNullableString(reader, "text"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotstexte.Add(angebotstext);
            }
            return angebotstexte;
        }
    }
}
