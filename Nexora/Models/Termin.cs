namespace Nexora.Models
{
    public class Termin
    {
        public int? Id { get; set; }
        public int? Projekt_id { get; set; }
        public string? Status { get; set; }
        public DateTime? Datum { get; set; }
        public int? Mandant_id { get; set; }
        public string? Typ { get; set; }
        public int? Kunden_id { get; set; }
        public int? Objekt_id { get; set; }

    }
}
