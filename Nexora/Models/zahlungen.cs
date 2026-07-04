namespace Nexora.Models
{
    public class zahlungen
    {
        public int id { get; set; }
        public int rechnungs_id { get; set; }
        public DateTime datum { get; set; }
        public int kunden_id { get; set; }
        public char[] status { get; set; }
        public double betrag { get; set; }
        public int mandant_id { get; set; }
    }
}
