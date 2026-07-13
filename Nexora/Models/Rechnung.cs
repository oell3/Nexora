namespace Nexora.Models
{
    public class Rechnung
    {
        public int? Id { get; set; }

        public int? Angebots_id { get; set; }

        public string? Rechnungsnummer { get; set; }

        public double? Betrag { get; set; }

        public int? Objekt_id { get; set; }

        public int? Kunden_id { get; set; }

        public string? Status { get; set; }

        public DateTime? Datum { get; set; }

        public int? Projekt_id { get; set; }

        public int? Mandant_id { get; set; }
    }
}
