namespace Nexora.Models
{
    public class ansprechpartner
    {
        public int id { get; set; }
        public int kunden_id { get; set; }
        public char[] anrede { get; set; }
        public string vorname { get; set; }
        public string nachname { get; set; }
        public char[] mail { get; set; }
        public char[] telefon   { get; set; }
        public char[] fax {  get; set; }
        public char[] funktion { get; set; }
        public Boolean hauptansprechpartner { get; set; }
        public int objekt_id { get; set; }
        public char[] mobil { get; set; }
        public int mandant_id { get; set; }
    }
}
