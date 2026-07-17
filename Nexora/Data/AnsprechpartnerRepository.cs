using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class AnsprechpartnerRepository(Database db, Helpers helpers)
    {

        public async Task<List<Ansprechpartner>> GetAnsprechpartnerAsync(int mandant_id)
        {
            var ansprechpartnerList = new List<Ansprechpartner>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.ansprechpartner WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var ansprechpartner = new Ansprechpartner
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Kunden_id = helpers.GetNullableInt(reader, "kunden_id"),
                    Anrede = helpers.GetNullableString(reader, "anrede"),
                    Vorname = helpers.GetNullableString(reader, "vorname"),
                    Nachname = helpers.GetNullableString(reader, "nachname"),
                    Mail = helpers.GetNullableString(reader, "mail"),
                    Telefon = helpers.GetNullableString(reader, "telefon"),
                    Fax = helpers.GetNullableString(reader, "fax"),
                    Funktion = helpers.GetNullableString(reader, "funktion"),
                    Hauptansprechpartner = helpers.GetNullableBoolean(reader, "hauptansprechpartner"),
                    Objekt_id = helpers.GetNullableInt(reader, "objekt_id"),
                    Mobil = helpers.GetNullableString(reader, "mobil"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                ansprechpartnerList.Add(ansprechpartner);
            }
            return ansprechpartnerList;
        }

    }
}
