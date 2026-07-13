namespace Nexora.Models
{
    public class Katalog
    {
        public int? Id { get; set; }
        public string? Artikelnummer { get; set; }
        public string? Artikelname { get; set; }
        public double? Preis { get; set; }
        public int? Mandant_id { get; set; }
    }
}
