using Nexora.Models;
using Nexora.utils;
using Npgsql;
namespace Nexora.Data
{
    public class RechnungenRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;

        public RechnungenRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }

        public async Task<List<Rechnung>> GetRechnungenProjekteAsync(int projekt_id, int mandant_id)
        {
            using var conn = _db.CreateConnection();
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
                    Id = _helpers.GetNullableInt(reader, "id"),
                    Angebots_id = _helpers.GetNullableInt(reader, "angebots_id"),
                    Mandant_id = _helpers.GetNullableInt(reader, "mandant_id"),
                    Rechnungsnummer = _helpers.GetNullableString(reader, "rechnungsnummer"),
                    Objekt_id = _helpers.GetNullableInt(reader, "objekt_id"),
                    Kunden_id = _helpers.GetNullableInt(reader, "kunden_id"),
                    Status = _helpers.GetNullableString(reader, "status"),
                    Datum = _helpers.GetNullableDateTime(reader, "datum"),
                    Betrag = _helpers.GetNullableDouble(reader, "betrag")
                };
                rechnungenList.Add(rechnung);
            }
            return rechnungenList;
        }

    }
}
