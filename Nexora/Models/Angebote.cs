namespace Nexora.Models
{
    public class Angebote
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string angebotsnummer { get; set; }
        public int objekt_id { get; set; }
        public int kunden_id { get; set; }
        public double betrag { get; set; }
        public string status {  get; set; }
        public int projekt_id { get; set; }
        public string einleitung { get; set; }
        public string schluss { get; set; }
        public string anrede { get; set; }
        public int mandant_id { get; set; }
    }
}
