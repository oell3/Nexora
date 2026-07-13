using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class MandantRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;
        public MandantRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }
        public async Task<List<Mandant>> GetMandantenAsync()
        {
            var mandantenList = new List<Mandant>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.mandant", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var mandant = new Mandant
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Name = _helpers.GetNullableString(reader, "name"),
                    Slug = _helpers.GetNullableString(reader, "slug"),
                    Email = _helpers.GetNullableString(reader, "email"),
                    Telefon = _helpers.GetNullableString(reader, "telefon"),
                    Tarif = _helpers.GetNullableString(reader, "tarif"),
                    Status = _helpers.GetNullableString(reader, "status"),
                    IBAN = _helpers.GetNullableString(reader, "iban"),
                    BIC = _helpers.GetNullableString(reader, "bic")
                };
                mandantenList.Add(mandant);
            }
            return mandantenList;
        }
    }
}
