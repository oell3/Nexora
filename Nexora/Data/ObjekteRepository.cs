using Nexora.Models;
using Nexora.utils;
using Npgsql;
namespace Nexora.Data
{
    public class ObjekteRepository(Database db, Helpers helpers)
    {
        
        public async Task<List<Objekt>> GetObjekteAsync(int mandant_id)
        {
            using var conn = db.CreateConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.objekte WHERE mandant_id = @mandant_id", conn);
            conn.Open();
            cmd.Parameters.AddWithValue("@mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            var objekteList = new List<Objekt>();
            while (await reader.ReadAsync())
            {
                var objekte = new Objekt()
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Kunden_id = helpers.GetNullableInt(reader, "kunden_id"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id"),
                    Adresse = helpers.GetNullableString(reader, "adresse"),
                    Plz = helpers.GetNullableString(reader, "plz"),
                    Stadt = helpers.GetNullableString(reader, "stadt"),
                    Lat = helpers.GetNullableString(reader, "lat"),
                    Lng = helpers.GetNullableString(reader, "lng")
                };
                objekteList.Add(objekte);
            }
            return objekteList;
        }

    }
}