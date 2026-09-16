using System.Text.Json.Serialization;

namespace Nexora.Models
{
    public class Angebot
    {
        public int? Id { get; set; }
        public string? title { get; set; }
        public string? Angebotsnummer { get; set; }
        public int? objektId { get; set; }
        public int? kundenId { get; set; }
        public double? Betrag { get; set; }
        public DateTime? Datum { get; set; }
        public string? Status { get; set; }
        public int? projektId { get; set; }
        public string? einleitung { get; set; }
        public string? schluss { get; set; }
        public string? anrede { get; set; }
        public int? Mandant_id { get; set; }

        [JsonPropertyName("groups")]
        public List<AngebotGruppe>? AngebotGruppe { get; set; }
    }
}
