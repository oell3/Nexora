using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class AngebotPositionRepositorycs(Database db, Helpers helpers)
    {
        

        public async Task<List<AngebotPosition>> GetAngebotPositionenAsync(int mandant_id)
        {
            var angebotPositionen = new List<AngebotPosition>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.angebot_positionen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotPosition = new AngebotPosition
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Angebot_id = helpers.GetNullableInt(reader, "angebot_id"),
                    Group_id = helpers.GetNullableInt(reader, "group_id"),
                    Misch_id = helpers.GetNullableInt(reader, "misch_id"),
                    Parent_gruppe_id = helpers.GetNullableInt(reader, "parent_gruppe_id"),
                    Typ = helpers.GetNullableString(reader, "typ"),
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
                angebotPositionen.Add(angebotPosition);
            }
            return angebotPositionen;
        }

        public async Task<int> InsertPosition(Angebot angebot, AngebotGruppe gruppe, int gruppe_id,int mandant_id)
        {
            if(angebot.Id == null || gruppe.AngebotPosition == null)
            {
                throw new ArgumentException("Angebot ID cannot be null");
            }
            using var conn = db.CreateConnection();

            using var cmd = new Npgsql.NpgsqlCommand(@"
                INSERT INTO crm.angebot_positionen (angebot_id, group_id,misch_id,parent_misch_id,
                gruppe_id, parent_gruppe_id,typ,nr,bezeichnung,menge,einheit,ek,aufschlag,rabatt,ep,
                gp,sortierung,bemerkung,positionsart,mandant_id)
                VALUES (@angebot_id, @group_id, @misch_id, @parent_misch_id, @gruppe_id, @parent_gruppe_id, 
                @typ::angebot_positionen_typ, @nr, @bezeichnung, @menge, @einheit, @ek, @aufschlag, @rabatt, @ep, @gp, @sortierung, 
                @bemerkung, @positionsart, @mandant_id)
                RETURNING id", conn);

            // Best Practice: Verbindung einmal öffnen und für alle Inserts wiederverwenden
            await conn.OpenAsync();
            foreach (var position in gruppe.AngebotPosition)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("angebot_id", angebot.Id);
                cmd.Parameters.AddWithValue("misch_id", position.Misch_id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("parent_misch_id", position.parent_Misch_id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("group_id", gruppe_id);
                cmd.Parameters.AddWithValue("gruppe_id", position.Gruppe_id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("parent_gruppe_id", position.Parent_gruppe_id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("typ", position.Typ ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("nr", position.Nr ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("bezeichnung", position.Bezeichnung ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("menge", position.Menge ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("einheit", position.Einheit ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ek", position.Ek ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("aufschlag", position.Aufschlag ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("rabatt", position.Rabatt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ep", position.Ep ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("gp", position.Gp ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("sortierung", position.Sortierung ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("bemerkung", position.Bemerkung ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("positionsart", position.Positionsart ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("mandant_id", mandant_id);

                // RETURNING id -> ExecuteScalarAsync liefert das neue ID (long)
                var insertedId = (int)(long)await cmd.ExecuteScalarAsync();
                // TODO: bei Bedarf: speichere/verwende insertedId
            }
            await conn.CloseAsync();
            return 1;
        }
    }
}
