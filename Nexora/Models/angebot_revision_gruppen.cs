namespace Nexora.Models
{
    public class angebot_revision_gruppen
    {
        public int id { get; set; }
        public int revision_id { get; set; }
        
        public int gruppen_nr { get; set; }
        public char[] bezeichnung { get; set; }
        public int sortierung { get; set; }
        public int mandant_id { get; set; }
    }
}
