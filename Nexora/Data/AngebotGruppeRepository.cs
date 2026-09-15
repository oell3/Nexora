using Nexora.utils;
using System.ComponentModel;
using Nexora.Models;
using Nexora.Data;

namespace Nexora.Data
{
    public class AngebotGruppenRepository(Database db, Helpers helpers)
    {
     

        public async Task<List<Models.AngebotGruppe>> GetAngebotGruppenAsync(int mandant_id)
        {
            var angebotGruppen = new List<Models.AngebotGruppe>();
            using var conn = db.CreateConnection();
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM crm.angebot_gruppen WHERE mandant_id = @mandant_id", conn);
            cmd.Parameters.AddWithValue("mandant_id", mandant_id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var angebotGruppe = new Models.AngebotGruppe
                {
                    Id = helpers.GetNullableInt(reader, "id"),
                    Angebot_id = helpers.GetNullableInt(reader, "angebot_id"),
                    Gruppen_nr = helpers.GetNullableInt(reader, "gruppen_nr"),
                    Bezeichnung = helpers.GetNullableString(reader, "bezeichnung"),
                    Sortierung = helpers.GetNullableInt(reader, "sortierung"),
                    Mandant_id = helpers.GetNullableInt(reader, "mandant_id")
                };
                angebotGruppen.Add(angebotGruppe);
            }
            return angebotGruppen;
        }

        public  async Task<int> InsertGruppe(Angebot angebot, int mandant_id)
        {
            var Position = new AngebotPositionRepositorycs(db, helpers);

            if (angebot == null ||angebot.AngebotGruppe == null || angebot.AngebotGruppe.Count == 0)
            {
                throw new ArgumentNullException("angebot or angebot.AngebotGruppe is null or empty");
            }
            using var conn = db.CreateConnection();

            using var cmd = new Npgsql.NpgsqlCommand(@"
                INSERT INTO crm.angebot_gruppen (angebot_id, gruppen_nr, bezeichnung, sortierung, mandant_id)
                VALUES (@angebot_id, @gruppen_nr, @bezeichnung, @sortierung, @mandant_id)
                RETURNING id", conn);

            Console.WriteLine(angebot);

            // Best Practice: Verbindung einmal öffnen und für alle Gruppen wiederverwenden
            await conn.OpenAsync();
            foreach (var Gruppe in angebot.AngebotGruppe)
            {
                Console.WriteLine(Gruppe.Gruppen_nr);
                if (angebot.Id == null || Gruppe.Gruppen_nr == null || Gruppe.Bezeichnung == null || Gruppe.Sortierung == null)
                {
                    throw new ArgumentNullException("One or more required properties of Gruppe are null");
                }

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("angebot_id", angebot.Id);
                cmd.Parameters.AddWithValue("gruppen_nr", Gruppe.Gruppen_nr);
                cmd.Parameters.AddWithValue("bezeichnung", Gruppe.Bezeichnung);
                cmd.Parameters.AddWithValue("sortierung", Gruppe.Sortierung);
                cmd.Parameters.AddWithValue("mandant_id", mandant_id);

                int gruppen_id = (int)(long)await cmd.ExecuteScalarAsync();

                await Position.InsertPosition(angebot, Gruppe, gruppen_id, mandant_id);
            }
            await conn.CloseAsync();

            
            return 1;
        }
    }
}
