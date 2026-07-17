using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class ZahlungRepositroy(Database db, Helpers helpers)
    {
        public async Task<List<Zahlung>> GetZahlungenAsync(int mandant_id)
        {
            var zahlungList = new List<Zahlung>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.zahlung WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var zahlung = new Zahlung
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Rechnungs_id = helpers.GetNullableInt(reader, "rechnungs_id"),
                    Datum = helpers.GetNullableDateTime(reader, "datum"),
                    Kunden_id = helpers.GetNullableInt(reader, "kunden_id"),
                    Status = helpers.GetNullableString(reader, "status"),
                    Betrag = helpers.GetNullableDouble(reader, "betrag"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                zahlungList.Add(zahlung);
            }
            return zahlungList;
        }
    }
}
