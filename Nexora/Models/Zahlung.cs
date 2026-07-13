namespace Nexora.Models
{
    public class Zahlung
    {
        public int? Id { get; set; }
        public int? Rechnungs_id { get; set; }
        public DateTime? Datum { get; set; }
        public int? Kunden_id { get; set; }
        public string? Status { get; set; }
        public double? Betrag { get; set; }
        public int? Mandant_id { get; set; }
    }
}
