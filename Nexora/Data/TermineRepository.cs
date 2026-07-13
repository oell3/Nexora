using Nexora.Models;
using Nexora.utils;
using Npgsql;


namespace Nexora.Data
{
    public class TermineRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;
        public TermineRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<Termin>> GetTermineAsync(int mandant_id)
        {
            var termine = new List<Termin>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.termine WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var termin = new Termin
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Projekt_id = _helpers.GetNullableInt(reader, "projekt_id"),
                    Status = _helpers.GetNullableString(reader, "status"),
                    Typ = _helpers.GetNullableString(reader, "typ"),
                    Kunden_id = _helpers.GetNullableInt(reader, "kunden_id"),
                    Objekt_id = _helpers.GetNullableInt(reader, "objekt_id"),
                    Datum = _helpers.GetNullableDateTime(reader, "datum"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                termine.Add(termin);
            }
            return termine;
        }
    }
}
