namespace Nexora.Models
{
    public class Projektdetail
    {
        public int? Id { get; set; }
        public int? Projekt_id { get; set; }
        public float? Lon { get; set; }
        public float? Lat { get; set; }
        public string? Beschreibung { get; set; }
        public int? Objekt_id { get; set; }
        public DateTime? Datum { get; set; }
        public string? Name { get; set; }
        public int? Mandant_id { get; set; }

    }
}
