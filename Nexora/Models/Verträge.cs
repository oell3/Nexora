namespace Nexora.Models
{
    public class Verträge
    {
        public int Id { get; set; }

        public int objekt_id { get; set; }

        public int kunden_id { get; set; }

        public string pfad { get; set; }

        public char[] art { get; set; }

        public DateTime datum { get; set; }

        public int mandant_id { get; set; }

    }
}
