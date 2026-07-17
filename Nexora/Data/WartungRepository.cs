using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class WartungRepository(Database db, Helpers helpers)
    {

        public async Task<List<Wartung>> GetWartungenAsync(int mandant_id)
        {
            var wartungList = new List<Wartung>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.wartung WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var wartung = new Wartung
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Kunden_id = helpers.GetNullableInt(reader, "kunden_id"),
                    Objekt_id = helpers.GetNullableInt(reader, "objekt_id"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id"),
                    Typ = helpers.GetNullableString(reader, "typ"),
                    Erzeugerdaten = helpers.GetNullableString(reader, "erzeugerdaten"),
                    Datum = helpers.GetNullableDateTime(reader, "datum")
                };
                wartungList.Add(wartung);

            }
            return wartungList;
        }

    }
}
