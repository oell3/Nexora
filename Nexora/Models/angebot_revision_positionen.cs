namespace Nexora.Models
{
    public class angebot_revision_positionen
    {
        public int id { get; set; }
        public int revision_id { get; set; }
        public int group_id { get; set; }
        public int misch_id { get; set; }
        public int parent_gruppe_id { get; set; }
        public int typ { get; set; }

        public int nr { get; set; }
        public string bezeichnung { get; set; }
        public double menge { get; set; }
        public char[] einheit { get; set; }

        public double ek { get; set; }

        public double aufschlag { get; set; }

        public double rabatt { get; set; }
        public double ep { get; set; }

        public double gp { get; set; }
        public int sortierung { get; set; }
        public string bemerkung { get; set; }
        public char[] positionsart { get; set; }

        public int mandant_id { get;set; }

    }
}
