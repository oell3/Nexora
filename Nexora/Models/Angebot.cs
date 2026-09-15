using System.Text.Json.Serialization;

namespace Nexora.Models
{
    public class Angebot
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Angebotsnummer { get; set; }
        public int? Objekt_id { get; set; }
        public int? Kunden_id { get; set; }
        public double? Betrag { get; set; }
        public DateTime? Datum { get; set; }
        public string? Status { get; set; }
        public int? Projekt_id { get; set; }
        public string? Einleitung { get; set; }
        public string? Schluss { get; set; }
        public string? Anrede { get; set; }
        public int? Mandant_id { get; set; }

        [JsonPropertyName("groups")]
        public List<AngebotGruppe>? AngebotGruppe { get; set; }
    }
}
