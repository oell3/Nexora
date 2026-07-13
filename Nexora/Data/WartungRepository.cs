using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class WartungRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;

        public WartungRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<Wartung>> GetWartungenAsync(int mandant_id)
        {
            var wartungList = new List<Wartung>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.wartung WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var wartung = new Wartung
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Kunden_id = _helpers.GetNullableInt(reader, "kunden_id"),
                    Objekt_id = _helpers.GetNullableInt(reader, "objekt_id"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id"),
                    Typ = _helpers.GetNullableString(reader, "typ"),
                    Erzeugerdaten = _helpers.GetNullableString(reader, "erzeugerdaten"),
                    Datum = _helpers.GetNullableDateTime(reader, "datum")
                };
                wartungList.Add(wartung);

            }
            return wartungList;
        }

    }
}
