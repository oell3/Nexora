using Nexora.Models;
using Nexora.utils;
using Npgsql;
namespace Nexora.Data
{
    public class ObjekteRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;
        public ObjekteRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }
        public async Task<List<Objekt>> GetObjekteAsync(int mandant_id)
        {
            using var conn = _db.CreateConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.objekte WHERE mandant_id = @mandant_id", conn);
            conn.Open();
            cmd.Parameters.AddWithValue("@mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            var objekteList = new List<Objekt>();
            while (await reader.ReadAsync())
            {
                var objekte = new Objekt()
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Kunden_id = _helpers.GetNullableInt(reader, "kunden_id"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id"),
                    Adresse = _helpers.GetNullableString(reader, "adresse"),
                    Plz = _helpers.GetNullableString(reader, "plz"),
                    Stadt = _helpers.GetNullableString(reader, "stadt"),
                    Lat = _helpers.GetNullableString(reader, "lat"),
                    Lng = _helpers.GetNullableString(reader, "lng")
                };
                objekteList.Add(objekte);
            }
            return objekteList;
        }

    }
}