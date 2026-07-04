namespace Nexora.Models
{
    public class katalog
    {
        public int id { get; set; }
        public char[] artikelnummer { get; set; }
        public string artikelname { get; set; }
        public double preis { get; set; }
        public int mandant_id { get; set; }
    }
}
