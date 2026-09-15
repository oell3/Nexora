using Nexora.Models;
using Nexora.utils;
using Npgsql;
using System.Data.Common;

namespace Nexora.Data
{
    public class AngebotRepository(Database db, Helpers helpers)
    {
       
        public async Task<List<Angebot>> GetAngeboteProjekteAsync(int mandant_id = 1, int projekt_id = 1)
        {
            var angebote = new List<Angebot>();
            using var conn = db.CreateConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.angebote WHERE mandant_id = @mandant_id AND projekt_id = @projekt_id", conn);
            await conn.OpenAsync();

            cmd.Parameters.AddWithValue("mandant_id", mandant_id);

            cmd.Parameters.AddWithValue("projekt_id", projekt_id);

            await using var reader = await cmd.ExecuteReaderAsync();



            while (await reader.ReadAsync())
            {
                var angebot = new Angebot
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

        public async Task<int> InsertAngebot(Angebot angebot, int mandant_id, int user_id)
        {
            if (angebot == null)
            {
                return 0;
            }
            await using var conn = db.CreateConnection();
            await conn.OpenAsync();

            var angebotsnummer = await Angebotsnummer(mandant_id);

            using var cmd = new NpgsqlCommand(@"
        INSERT INTO crm.angebote (
            title, angebotsnummer, objekt_id, kunden_id,
            betrag, status, projekt_id, einleitung,
            schluss, anrede, mandant_id, user_id
        ) VALUES (
            @title, @angebotsnummer, @objekt_id, @kunden_id,
            @betrag, @status, @projekt_id, @einleitung,
            @schluss, @anrede, @mandant_id, @user_id
        ) RETURNING id", conn);


            cmd.Parameters.AddWithValue("title", angebot.Title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("angebotsnummer", angebotsnummer);
            cmd.Parameters.AddWithValue("objekt_id", angebot.Objekt_id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("kunden_id", angebot.Kunden_id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("betrag", angebot.Betrag ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("status", "in Bearbeitung");
            cmd.Parameters.AddWithValue("projekt_id", angebot.Projekt_id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("einleitung", angebot.Einleitung ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("schluss", angebot.Schluss ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("anrede", angebot.Anrede ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            cmd.Parameters.AddWithValue("user_id", user_id);

            angebot.Id = (int)(long)await cmd.ExecuteScalarAsync();

            var angebotGruppe = new AngebotGruppenRepository(db, helpers);

            await angebotGruppe.InsertGruppe(angebot, mandant_id);

            return 1;
        }

        private async Task<string> Angebotsnummer(int mandant_id)
        {
            var heute = DateTime.UtcNow.Date;
            using var conn = db.CreateConnection();
            var cmd = new NpgsqlCommand("Select COUNT(*) FROM crm.angebote WHERE mandant_id = @mandant_id AND datum = @datum", conn);

            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            cmd.Parameters.AddWithValue("datum", DateTime.UtcNow);

            await conn.OpenAsync();

            var angebot_anzahl = (int)await cmd.ExecuteNonQueryAsync();

            var neue_nummer = angebot_anzahl + 1;

            var angebot_nummer = $"{heute:yyyyMMdd}-{neue_nummer:D3}";

            return angebot_nummer;
        }
    }
}
