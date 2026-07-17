using Nexora.Models;
using Nexora.utils;
using Npgsql;


namespace Nexora.Data
{
    public class TermineRepository(Database db, Helpers helpers)
    {
        

        public async Task<List<Termin>> GetTermineAsync(int mandant_id)
        {
            var termine = new List<Termin>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.termine WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var termin = new Termin
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Projekt_id = helpers.GetNullableInt(reader, "projekt_id"),
                    Status = helpers.GetNullableString(reader, "status"),
                    Typ = helpers.GetNullableString(reader, "typ"),
                    Kunden_id = helpers.GetNullableInt(reader, "kunden_id"),
                    Objekt_id = helpers.GetNullableInt(reader, "objekt_id"),
                    Datum = helpers.GetNullableDateTime(reader, "datum"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                termine.Add(termin);
            }
            return termine;
        }
    }
}
