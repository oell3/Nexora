using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class AnsprechpartnerRepository
    {

        private readonly Database _db;
        private readonly Helpers _helpers;

        public AnsprechpartnerRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<Ansprechpartner>> GetAnsprechpartnerAsync(int mandant_id)
        {
            var ansprechpartnerList = new List<Ansprechpartner>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.ansprechpartner WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var ansprechpartner = new Ansprechpartner
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Kunden_id = _helpers.GetNullableInt(reader, "kunden_id"),
                    Anrede = _helpers.GetNullableString(reader, "anrede"),
                    Vorname = _helpers.GetNullableString(reader, "vorname"),
                    Nachname = _helpers.GetNullableString(reader, "nachname"),
                    Mail = _helpers.GetNullableString(reader, "mail"),
                    Telefon = _helpers.GetNullableString(reader, "telefon"),
                    Fax = _helpers.GetNullableString(reader, "fax"),
                    Funktion = _helpers.GetNullableString(reader, "funktion"),
                    Hauptansprechpartner = _helpers.GetNullableBoolean(reader, "hauptansprechpartner"),
                    Objekt_id = _helpers.GetNullableInt(reader, "objekt_id"),
                    Mobil = _helpers.GetNullableString(reader, "mobil"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                ansprechpartnerList.Add(ansprechpartner);
            }
            return ansprechpartnerList;
        }

    }
}
