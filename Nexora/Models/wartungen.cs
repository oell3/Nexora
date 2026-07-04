namespace Nexora.Models
{
    public class wartungen
    {
        public int id { get; set; }
        public int kunden_id { get; set; }
        public int objekt_id { get; set; }
        public int mandant_id { get; set; }
        public char[] typ { get; set; }
        public char[] erzeugerdaten { get; set; }
        public DateTime datum { get; set; }

    }
}
