using System.Text.Json.Serialization;

namespace Nexora.Models
{
    public class AngebotGruppe
    {
        public int? Id { get; set; }
        public int? Angebot_id { get; set; }

        public int? Gruppen_nr { get; set; }
        [JsonPropertyName("name")]
        public string? Bezeichnung { get; set; }

        [JsonPropertyName("sortierung")]
        public int? Sortierung { get; set; }
        public int? Mandant_id { get; set; }
        
        [JsonPropertyName("rows")]
        public List<AngebotPosition>? AngebotPosition { get; set; }

    }
}
