using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class KatalogRepository(Database db, Helpers helpers)
    {
        
        public async Task<List<Katalog>> GetKatalogAsync(int mandant_id)
        {
            var katalogList = new List<Katalog>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.katalog WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var katalog = new Katalog
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Artikelnummer = helpers.GetNullableString(reader, "artikelnummer"),
                    Artikelname = helpers.GetNullableString(reader, "artikelname"),
                    Preis = helpers.GetNullableDouble(reader, "preis"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                katalogList.Add(katalog);
            }
            return katalogList;
        }
    }
}
