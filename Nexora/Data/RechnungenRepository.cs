using Nexora.Models;
using Nexora.utils;
using Npgsql;
namespace Nexora.Data
{
    public class RechnungenRepository(Database db, Helpers helpers)
    {
        

        public async Task<List<Rechnung>> GetRechnungenProjekteAsync(int projekt_id, int mandant_id)
        {
            using var conn = db.CreateConnection();
            using var cmd = new NpgsqlCommand("SELECT * FROM crm.rechnungen WHERE projekt_id = @projekt_id AND mandant_id = @mandant_id", conn);
            conn.Open();
            cmd.Parameters.AddWithValue("@projekt_id", projekt_id);
            cmd.Parameters.AddWithValue("@mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            var rechnungenList = new List<Rechnung>();
            while (await reader.ReadAsync())
            {
                var rechnung = new Rechnung()
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Angebots_id = helpers.GetNullableInt(reader, "angebots_id"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id"),
                    Rechnungsnummer = helpers.GetNullableString(reader, "rechnungsnummer"),
                    Objekt_id = helpers.GetNullableInt(reader, "objekt_id"),
                    Kunden_id = helpers.GetNullableInt(reader, "kunden_id"),
                    Status = helpers.GetNullableString(reader, "status"),
                    Datum = helpers.GetNullableDateTime(reader, "datum"),
                    Betrag = helpers.GetNullableDouble(reader, "betrag")
                };
                rechnungenList.Add(rechnung);
            }
            return rechnungenList;
        }

    }
}
