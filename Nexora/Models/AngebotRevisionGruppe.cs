namespace Nexora.Models
{
    public class AngebotRevisionGruppe
    {
        public int? Id { get; set; }
        public int? Revision_id { get; set; }

        public int? Gruppen_nr { get; set; }
        public string? Bezeichnung { get; set; }
        public int? Sortierung { get; set; }
        public int? Mandant_id { get; set; }
    }
}
