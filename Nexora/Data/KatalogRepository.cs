using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class KatalogRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;
        public KatalogRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }
        public async Task<List<Katalog>> GetKatalogAsync(int mandant_id)
        {
            var katalogList = new List<Katalog>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.katalog WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var katalog = new Katalog
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Artikelnummer = _helpers.GetNullableString(reader, "artikelnummer"),
                    Artikelname = _helpers.GetNullableString(reader, "artikelname"),
                    Preis = _helpers.GetNullableDouble(reader, "preis"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                katalogList.Add(katalog);
            }
            return katalogList;
        }
