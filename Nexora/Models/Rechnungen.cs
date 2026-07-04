namespace Nexora.Models
{
    public class Rechnungen
    {
        public int id { get; set; }

        public int angebote_id { get; set; }

        public char[] rechnungsnummer { get; set; }

        public double betrag { get; set; }

        public int objekt_id { get; set; }

        public int kunden_id { get; set; }

        public char[] status { get; set; }

        public DateTime datum { get; set; }

        public int projekt_id { get; set; }

        public int mandant_id { get; set; }
    }
}
