namespace Nexora.Models
{
    public class Objekte
    {
        public string Id { get; set; }
        public int kunden_id { get; set; }
        public int mandant_id { get; set; }

        public char[] adresse {  get; set; }

        public char[] plz { get; set; }

        public char[] stadt { get; set; }

        public char[] lat { get; set; }

        public char[] lng { get; set; }
    }
}
