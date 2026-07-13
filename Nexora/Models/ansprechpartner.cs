namespace Nexora.Models
{
    public class Ansprechpartner
    {
        public int? Id { get; set; }
        public int? Kunden_id { get; set; }
        public string? Anrede { get; set; }
        public string? Vorname { get; set; }
        public string? Nachname { get; set; }
        public string? Mail { get; set; }
        public string? Telefon { get; set; }
        public string? Fax { get; set; }
        public string? Funktion { get; set; }
        public Boolean? Hauptansprechpartner { get; set; }
        public int? Objekt_id { get; set; }
        public string? Mobil { get; set; }
        public int? Mandant_id { get; set; }
    }
}
