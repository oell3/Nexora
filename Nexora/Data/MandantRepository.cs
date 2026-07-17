using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class MandantRepository(Database db, Helpers helpers)
    {
       
        public async Task<List<Mandant>> GetMandantenAsync()
        {
            var mandantenList = new List<Mandant>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.mandant", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var mandant = new Mandant
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Name = helpers.GetNullableString(reader, "name"),
                    Slug = helpers.GetNullableString(reader, "slug"),
                    Email = helpers.GetNullableString(reader, "email"),
                    Telefon = helpers.GetNullableString(reader, "telefon"),
                    Tarif = helpers.GetNullableString(reader, "tarif"),
                    Status = helpers.GetNullableString(reader, "status"),
                    IBAN = helpers.GetNullableString(reader, "iban"),
                    BIC = helpers.GetNullableString(reader, "bic")
                };
                mandantenList.Add(mandant);
            }
            return mandantenList;
        }
    }
}
