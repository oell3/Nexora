namespace Nexora.Models
{
    public class Vertraeg
    {
        public int? Id { get; set; }

        public int? Objekt_id { get; set; }

        public int? Kunden_id { get; set; }

        public string? Pfad { get; set; }

        public string? Art { get; set; }

        public DateTime? Datum { get; set; }

        public int? Mandant_id { get; set; }

    }
}
