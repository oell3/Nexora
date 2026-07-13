using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotRevisionGruppenRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;
        public AngebotRevisionGruppenRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }
        public async Task<List<AngebotRevisionGruppe>> GetAngebotRevisionGruppenAsync(int mandant_id)
        {
            var angebotRevisionGruppen = new List<AngebotRevisionGruppe>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebot_revision_gruppen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotRevisionGruppe = new AngebotRevisionGruppe
                {
                    Id = _helpers.GetNullableInt(reader, "id") ?? 0,
                    Revision_id = _helpers.GetNullableInt(reader, "revision_id"),
                    Gruppen_nr = _helpers.GetNullableInt(reader, "gruppen_nr"),
                    Bezeichnung = _helpers.GetNullableString(reader, "bezeichnung"),
                    Sortierung = _helpers.GetNullableInt(reader, "sortierung"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotRevisionGruppen.Add(angebotRevisionGruppe);
            }
            return angebotRevisionGruppen;
        }
    }
}
