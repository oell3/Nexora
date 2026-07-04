namespace Nexora.Models
{
    public class Projekte
    {
        public int id { get; set; }
        public int kunden_id { get; set; }
        public int objekt_id { get; set; }
        public int mandant_id { get; set; }
        public char[] status { get; set; }
        public int termin_id { get; set; }
        public char[] name { get; set; }

        public DateTime DateTime { get; set; }

        public int ansprechpartner_id { get; set; }

        public int mandant_id { get; set; }
    }
}
