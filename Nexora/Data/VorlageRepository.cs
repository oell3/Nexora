using Nexora.Models;
using Nexora.utils;

namespace Nexora.Data
{
    public class VorlageRepository
    {
        private readonly Database _db;
        private readonly Helpers _helpers;
        public VorlageRepository(Database db, Helpers helpers)
        {
            _db = db;
            _helpers = helpers;
        }
        public async Task<List<Vorlage>> GetVorlagenAsync(int mandant_id)
        {
            var vorlageList = new List<Vorlage>();
            using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.vorlagen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var vorlage = new Vorlage
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Typ = reader.GetString(reader.GetOrdinal("typ")),
                    Text = reader.GetString(reader.GetOrdinal("text")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Mandant_id = reader.GetInt32(reader.GetOrdinal("mandant_id"))
                };
                vorlageList.Add(vorlage);
            }
            return vorlageList;
        }
    }
}
