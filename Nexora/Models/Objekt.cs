namespace Nexora.Models
{
    public class Objekt
    {
        public int? Id { get; set; }
        public int? Kunden_id { get; set; }
        public int? Mandant_id { get; set; }

        public string? Adresse { get; set; }

        public string? Plz { get; set; }

        public string? Stadt { get; set; }

        public string? Lat { get; set; }

        public string? Lng { get; set; }
    }
}
