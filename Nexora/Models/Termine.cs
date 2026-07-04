namespace Nexora.Models
{
    public class Termine
    {
        public int id { get; set; }
        public int projekt_id { get; set; }
        public char[] status { get; set; }
        public DateTime datum { get; set; }
        public int mandant_id { get; set; }
        public char[] typ { get; set; }
        public int kunden_id { get; set; }
        public int objekt_id { get; set; }

    }
}
