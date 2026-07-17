using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class KundenRepository(Database db, Helpers helpers)
    {
       
        public async Task<List<Kunde>> GetKundenAsync( int mandant_id)
        {
            using var conn = db.CreateConnection();
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
                    Typ = helpers.GetNullableString(reader, "typ"),
                    Name = helpers.GetNullableString(reader, "name"),
                    Adresse = helpers.GetNullableString(reader, "adresse"),
                    Plz = helpers.GetNullableString(reader, "plz"),
                    Stadt = helpers.GetNullableString(reader, "stadt"),
                    Mail = helpers.GetNullableString(reader, "mail"),
                    Telefon = helpers.GetNullableString(reader, "telefon"),
                    Fax = helpers.GetNullableString(reader, "fax"),
                    Iban = helpers.GetNullableString(reader, "iban"),
                    Bic = helpers.GetNullableString(reader, "bic"),
                    Steueridentifikationsnummer = helpers.GetNullableString(reader, "steueridentifikationsnummer"),
                    Lat = helpers.GetNullableString(reader, "lat"),
                    Lon = helpers.GetNullableString(reader, "lon"),

                    Mandant_id = reader.GetInt32(reader.GetOrdinal("mandant_id"))
                    // Map other properties as needed
                };
                kunden.Add(kunde);
            }
            return kunden;
        }

    }
}
