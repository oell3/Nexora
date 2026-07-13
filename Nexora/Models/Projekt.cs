namespace Nexora.Models
{
    public class Projekt
    {
        public int? Id { get; set; }
        public int? Kunden_id { get; set; }
        public int? Objekt_id { get; set; }
        public string? Status { get; set; }
        public int? Termin_id { get; set; }
        public string? Name { get; set; }

        public DateTime? DateTime { get; set; }

        public int? Ansprechpartner_id { get; set; }

        public int? Mandant_id { get; set; }
    }
}
