using Nexora.utils;

namespace Nexora.Data
{
    public class AngebotGruppenRepository(Database db, Helpers helpers)
    {
     

        public async Task<List<Models.AngebotGruppe>> GetAngebotGruppenAsync(int mandant_id)
        {
            var angebotGruppen = new List<Models.AngebotGruppe>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.angebot_gruppen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotGruppe = new Models.AngebotGruppe
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Angebot_id = helpers.GetNullableInt(reader, "angebot_id"),
                    Gruppen_nr = helpers.GetNullableInt(reader, "gruppen_nr"),
                    Bezeichnung = helpers.GetNullableString(reader, "bezeichnung"),
                    Sortierung = helpers.GetNullableInt(reader, "sortierung"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotGruppen.Add(angebotGruppe);
            }
            return angebotGruppen;
        }
    }
}
