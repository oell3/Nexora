using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class AngeboteRepository(Database db, Helpers helpers)
    {
       
        public async Task<List<Angebote>> GetAngeboteProjekteAsync(int mandant_id = 1, int projekt_id = 1)
        {
            var angebote = new List<Angebote>();
            await using var conn = db.CreateConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebote WHERE mandant_id = @mandant_id AND projekt_id = @projekt_id", conn);
            await conn.OpenAsync();

            cmd.Parameters.AddWithValue("mandant_id", mandant_id);

            cmd.Parameters.AddWithValue("projekt_id", projekt_id);

            await using var reader = await cmd.ExecuteReaderAsync();



            while (await reader.ReadAsync())
            {
                var angebot = new Angebote
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Title = helpers.GetNullableString(reader, "title"),
                    Angebotsnummer = helpers.GetNullableString(reader, "angebotsnummer"),
                    Objekt_id = reader.GetInt32(reader.GetOrdinal("objekt_id")),
                    Kunden_id = reader.GetInt32(reader.GetOrdinal("kunden_id")),
                    Betrag = reader.GetDouble(reader.GetOrdinal("betrag")),
                    Datum = helpers.GetNullableDateTime(reader, "datum"),
                    Status = helpers.GetNullableString(reader, "status"),
                    Projekt_id = reader.GetInt32(reader.GetOrdinal("projekt_id")),
                    Einleitung = helpers.GetNullableString(reader, "einleitung"),
                    Schluss = helpers.GetNullableString(reader, "schluss"),
                    Anrede = helpers.GetNullableString(reader, "anrede"),
                    Mandant_id = reader.GetInt32(reader.GetOrdinal("mandant_id"))
                };
                angebote.Add(angebot);
            }
            return angebote;
        }
    }
}
