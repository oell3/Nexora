using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotRevisionGruppenRepository(Database db, Helpers helpers)
    {
       
        public async Task<List<AngebotRevisionGruppe>> GetAngebotRevisionGruppenAsync(int mandant_id)
        {
            var angebotRevisionGruppen = new List<AngebotRevisionGruppe>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebot_revision_gruppen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotRevisionGruppe = new AngebotRevisionGruppe
                {
                    Id = helpers.GetNullableInt(reader, "id") ?? 0,
                    Revision_id = helpers.GetNullableInt(reader, "revision_id"),
                    Gruppen_nr = helpers.GetNullableInt(reader, "gruppen_nr"),
                    Bezeichnung = helpers.GetNullableString(reader, "bezeichnung"),
                    Sortierung = helpers.GetNullableInt(reader, "sortierung"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotRevisionGruppen.Add(angebotRevisionGruppe);
            }
            return angebotRevisionGruppen;
        }
    }
}
