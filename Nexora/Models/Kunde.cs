namespace Nexora.Models
{
    public class Kunde
    {
        public int Id { get; set; }
        public string typ { get; set; }
        public string name { get; set; }
        public string adresse { get; set; }
        public string plz { get; set; }
        public string stadt { get; set; }   
        public string mail { get; set; }
        public char[] telefon { get; set; }
        public char[] fax { get; set; }
        public char[] iban { get; set; }
        public char[] bic { get; set; }

        public char[] steueridentifikationsnummer { get; set; }

        public char[] lat { get; set; }
        public char[] lng { get; set; }
        public int mandant_id { get; set; }
    }
}
