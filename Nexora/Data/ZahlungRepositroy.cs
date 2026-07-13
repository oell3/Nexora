using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class ZahlungRepositroy
    {
        private readonly Database _db;
        private readonly Helpers _helpers;

        public ZahlungRepositroy(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }
        public async Task<List<Zahlung>> GetZahlungenAsync(int mandant_id)
        {
            var zahlungList = new List<Zahlung>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.zahlung WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var zahlung = new Zahlung
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Rechnungs_id = _helpers.GetNullableInt(reader, "rechnungs_id"),
                    Datum = _helpers.GetNullableDateTime(reader, "datum"),
                    Kunden_id = _helpers.GetNullableInt(reader, "kunden_id"),
                    Status = _helpers.GetNullableString(reader, "status"),
                    Betrag = _helpers.GetNullableDouble(reader, "betrag"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                zahlungList.Add(zahlung);
            }
            return zahlungList;
        }
    }
}
