using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class AngebotRevisionPositionRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;

        public AngebotRevisionPositionRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<AngebotRevisionPosition>> GetAngebotRevisionPositionenAsync(int mandant_id)
        {
            var angebotRevisionPositionen = new List<AngebotRevisionPosition>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebot_revision_positionen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotRevisionPosition = new AngebotRevisionPosition
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Revision_id = _helpers.GetNullableInt(reader, "revision_id"),
                    Group_id = _helpers.GetNullableInt(reader, "group_id"),
                    Misch_id = _helpers.GetNullableInt(reader, "misch_id"),
                    Parent_gruppe_id = _helpers.GetNullableInt(reader, "parent_gruppe_id"),
                    Typ = _helpers.GetNullableInt(reader, "typ"),
                    Nr = _helpers.GetNullableInt(reader, "position_nr"),
                    Bezeichnung = _helpers.GetNullableString(reader, "bezeichnung"),
                    Menge = _helpers.GetNullableDouble(reader, "menge"),
                    Einheit = _helpers.GetNullableString(reader, "einheit"),
                    Ek = _helpers.GetNullableDouble(reader, "einzelpreis"),
                    Aufschlag = _helpers.GetNullableDouble(reader, "aufschlag"),
                    Rabatt = _helpers.GetNullableDouble(reader, "rabatt"),
                    Ep = _helpers.GetNullableDouble(reader, "ep"),
                    Gp = _helpers.GetNullableDouble(reader, "gp"),
                    Sortierung = _helpers.GetNullableInt(reader, "sortierung"),
                    Bemerkung = _helpers.GetNullableString(reader, "bemerkung"),
                    Positionsart = _helpers.GetNullableString(reader, "positionsart"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotRevisionPositionen.Add(angebotRevisionPosition);
            }
            return angebotRevisionPositionen;
        }
    }
}
