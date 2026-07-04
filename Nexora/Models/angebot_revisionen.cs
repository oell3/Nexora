namespace Nexora.Models
{
    public class angebot_revisionen
    {
        public int id { get; set; }
        public int angebot_id { get; set; }
        public int revisionsnummer { get; set; }
        public DateTime erstellt_am { get; set; }
        public int mandant_id { get; set; }
    }
}
