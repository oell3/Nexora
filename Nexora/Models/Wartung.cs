namespace Nexora.Models
{
    public class Wartung
    {
        public int? Id { get; set; }
        public int? Kunden_id { get; set; }
        public int? Objekt_id { get; set; }
        public int? Mandant_id { get; set; }
        public string? Typ { get; set; }
        public string? Erzeugerdaten { get; set; }
        public DateTime? Datum { get; set; }

    }
}
