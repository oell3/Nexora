namespace Nexora.Models
{
    public class AngebotPosition
    {
        public int? Id { get; set; }
        public int? Angebot_id { get; set; }
        public int? Group_id { get; set; }
        public int? Gruppe_id { get; set; }
        public int? Misch_id { get; set; }
        public int? Parent_gruppe_id { get; set; }
        public int? parent_Misch_id { get; set; }
        public string? Typ { get; set; }

        public int? Nr { get; set; }
        public string? Bezeichnung { get; set; }
        public double? Menge { get; set; }
        public string? Einheit { get; set; }

        public double? Ek { get; set; }

        public double? Aufschlag { get; set; }

        public double? Rabatt { get; set; }
        public double? Ep { get; set; }

        public double? Gp { get; set; }
        public int? Sortierung { get; set; }
        public string? Bemerkung { get; set; }
        public string? Positionsart { get; set; }

        public int? Mandant_id { get; set; }

    }
}
