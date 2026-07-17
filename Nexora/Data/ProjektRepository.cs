using Nexora.Models;
using Nexora.utils;
using Npgsql;

namespace Nexora.Data
{
    public class ProjektRepository(Database db, Helpers helpers)
    {
        

        public async Task<List<Projekt>> GetProjekteAsync(int projekt_id, int mandant_id)
        {
            var projekte = new List<Projekt>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.projekte WHERE id = @projekt_id AND mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("projekt_id", projekt_id);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var projekt = new Projekt
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Kunden_id = helpers.GetNullableInt(reader, "kunden_id"),
                    Objekt_id = helpers.GetNullableInt(reader, "objekt_id"),
                    Termin_id = helpers.GetNullableInt(reader, "termin_id"),
                    Name = helpers.GetNullableString(reader, "name"),
                    DateTime = helpers.GetNullableDateTime(reader, "datum"),
                    Ansprechpartner_id = helpers.GetNullableInt(reader, "ansprechpartner_id"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                projekte.Add(projekt);
            }
            return projekte;
        }

        public async Task<List<Projekt>> GetAllProjekbyMandantAsync(int mandant_id)
        {
            var projekte = new List<Projekt>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.projekte WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var projekt = new Projekt
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Kunden_id = helpers.GetNullableInt(reader, "kunden_id"),
                    Objekt_id = helpers.GetNullableInt(reader, "objekt_id"),
                    Termin_id = helpers.GetNullableInt(reader, "termin_id"),
                    Name = helpers.GetNullableString(reader, "name"),
                    DateTime = helpers.GetNullableDateTime(reader, "datum"),
                    Ansprechpartner_id = helpers.GetNullableInt(reader, "ansprechpartner_id"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                projekte.Add(projekt);
            }
            return projekte;
        }

    }
}
