using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class ProjektdetailRepositroy(Database db, Helpers helpers)
    {

        public async Task<List<Projektdetail>> GetProjektdetailAsync(int mandant_id)
        {
            var projektdetailList = new List<Projektdetail>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.projektdetail WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var projektdetail = new Projektdetail
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Projekt_id = helpers.GetNullableInt(reader, "projekt_id"),
                    Lon = helpers.GetNullableFloat(reader, "lon"),
                    Lat = helpers.GetNullableFloat(reader, "lat"),
                    Beschreibung = helpers.GetNullableString(reader, "beschreibung"),
                    Objekt_id = helpers.GetNullableInt(reader, "objekt_id"),
                    Datum = helpers.GetNullableDateTime(reader, "datum"),
                    Name = helpers.GetNullableString(reader, "name"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")

                };
                projektdetailList.Add(projektdetail);

            }
            return projektdetailList;
        }

    }
}
