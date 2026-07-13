namespace Nexora.Models
{
    public class Kunde
    {
        public int? Id { get; set; }
        public string? Typ { get; set; }
        public string? Name { get; set; }
        public string? Adresse { get; set; }
        public string? Plz { get; set; }
        public string? Stadt { get; set; }
        public string? Mail { get; set; }
        public string? Telefon { get; set; }
        public string? Fax { get; set; }
        public string? Iban { get; set; }
        public string? Bic { get; set; }

        public string? Steueridentifikationsnummer { get; set; }

        public string? Lat { get; set; }
        public string? Lon { get; set; }
        public int? Mandant_id { get; set; }
    }
}
