using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class KundenRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;
        public KundenRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }
        public async Task<List<Kunde>> GetKundenAsync(int projekt_id, int mandant_id)
        {
            using var conn = _db.CreateConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.kunde WHERE mandant_id = @mandant_id", conn);
            conn.Open();
            cmd.Parameters.AddWithValue("@mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            var kunden = new List<Kunde>();
            while (await reader.ReadAsync())
            {
                var kunde = new Kunde()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Typ = _helpers.GetNullableString(reader, "typ"),
                    Name = _helpers.GetNullableString(reader, "name"),
                    Adresse = _helpers.GetNullableString(reader, "adresse"),
                    Plz = _helpers.GetNullableString(reader, "plz"),
                    Stadt = _helpers.GetNullableString(reader, "stadt"),
                    Mail = _helpers.GetNullableString(reader, "mail"),
                    Telefon = _helpers.GetNullableString(reader, "telefon"),
                    Fax = _helpers.GetNullableString(reader, "fax"),
                    Iban = _helpers.GetNullableString(reader, "iban"),
                    Bic = _helpers.GetNullableString(reader, "bic"),
                    Steueridentifikationsnummer = _helpers.GetNullableString(reader, "steueridentifikationsnummer"),
                    Lat = _helpers.GetNullableString(reader, "lat"),
                    Lon = _helpers.GetNullableString(reader, "lon"),

                    Mandant_id = reader.GetInt32(reader.GetOrdinal("mandant_id"))
                    // Map other properties as needed
                };
                kunden.Add(kunde);
            }
            return kunden;
        }

    }
}
