using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class VorlageRepository(Database db, Helpers helpers)
    {
        
        public async Task<List<Vorlage>> GetVorlagenAsync(int mandant_id)
        {
            var vorlageList = new List<Vorlage>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.vorlagen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var vorlage = new Vorlage
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Typ = helpers.GetNullableString(reader, "typ"),
                    Text = helpers.GetNullableString(reader,"text"),
                    Name = helpers.GetNullableString(reader,"name"),
                    Mandant_id = helpers.GetNullableInt(reader,"mandant_id")
                };
                vorlageList.Add(vorlage);
            }
            return vorlageList;
        }
    }
}
