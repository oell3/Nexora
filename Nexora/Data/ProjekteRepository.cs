using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class ProjekteRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;
        public ProjekteRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<Projekt>> GetProjekteAsync(int projekt_id, int mandant_id)
        {
            var projekte = new List<Projekt>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.projekte WHERE id = @projekt_id AND mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("projekt_id", projekt_id);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var projekt = new Projekt
                {
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Kunden_id = _helpers.GetNullableInt(reader, "kunden_id"),
                    Objekt_id = _helpers.GetNullableInt(reader, "objekt_id"),
                    Termin_id = _helpers.GetNullableInt(reader, "termin_id"),
                    Name = _helpers.GetNullableString(reader, "name"),
                    DateTime = _helpers.GetNullableDateTime(reader, "datum"),
                    Ansprechpartner_id = _helpers.GetNullableInt(reader, "ansprechpartner_id"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id")
                };
                projekte.Add(projekt);
            }
            return projekte;
        }

    }
}
