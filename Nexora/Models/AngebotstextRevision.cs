namespace Nexora.Models
{
    public class AngebotstextRevision
    {
        public int? Id { get; set; }
        public int? Angebot_id { get; set; }
        public int? Revisionsnummer { get; set; }
        public DateTime? Erstellt_am { get; set; }
        public int? Mandant_id { get; set; }
    }
}
