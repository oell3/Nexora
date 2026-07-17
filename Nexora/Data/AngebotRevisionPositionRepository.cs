using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotRevisionPositionRepository(Database db, Helpers helpers)
    {
        

        public async Task<List<AngebotRevisionPosition>> GetAngebotRevisionPositionenAsync(int mandant_id)
        {
            var angebotRevisionPositionen = new List<AngebotRevisionPosition>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebot_revision_positionen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotRevisionPosition = new AngebotRevisionPosition
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Revision_id = helpers.GetNullableInt(reader, "revision_id"),
                    Group_id = helpers.GetNullableInt(reader, "group_id"),
                    Misch_id = helpers.GetNullableInt(reader, "misch_id"),
                    Parent_gruppe_id = helpers.GetNullableInt(reader, "parent_gruppe_id"),
                    Typ = helpers.GetNullableInt(reader, "typ"),
                    Nr = helpers.GetNullableInt(reader, "position_nr"),
                    Bezeichnung = helpers.GetNullableString(reader, "bezeichnung"),
                    Menge = helpers.GetNullableDouble(reader, "menge"),
                    Einheit = helpers.GetNullableString(reader, "einheit"),
                    Ek = helpers.GetNullableDouble(reader, "einzelpreis"),
                    Aufschlag = helpers.GetNullableDouble(reader, "aufschlag"),
                    Rabatt = helpers.GetNullableDouble(reader, "rabatt"),
                    Ep = helpers.GetNullableDouble(reader, "ep"),
                    Gp = helpers.GetNullableDouble(reader, "gp"),
                    Sortierung = helpers.GetNullableInt(reader, "sortierung"),
                    Bemerkung = helpers.GetNullableString(reader, "bemerkung"),
                    Positionsart = helpers.GetNullableString(reader, "positionsart"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotRevisionPositionen.Add(angebotRevisionPosition);
            }
            return angebotRevisionPositionen;
        }
    }
}
