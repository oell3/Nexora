using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class ProjektdetailRepositroy
    {
        private readonly Database _db;
        private readonly Helpers _helpers;

        public ProjektdetailRepositroy(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<Projektdetail>> GetProjektdetailAsync(int mandant_id)
        {
            var projektdetailList = new List<Projektdetail>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.projektdetail WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var projektdetail = new Projektdetail
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Projekt_id = _helpers.GetNullableInt(reader, "projekt_id"),
                    Lon = _helpers.GetNullableFloat(reader, "lon"),
                    Lat = _helpers.GetNullableFloat(reader, "lat"),
                    Beschreibung = _helpers.GetNullableString(reader, "beschreibung"),
                    Objekt_id = _helpers.GetNullableInt(reader, "objekt_id"),
                    Datum = _helpers.GetNullableDateTime(reader, "datum"),
                    Name = _helpers.GetNullableString(reader, "name"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")

                };
                projektdetailList.Add(projektdetail);

            }
            return projektdetailList;
        }

    }
}
