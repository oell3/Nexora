namespace Nexora.Models
{
    public class mandant
    {
        public int Id { get; set; }
        public string name { get; set; }
        public char[] slug { get; set; }
        public char[] email { get; set; }

        public char[] telefon { get; set; }
        public char[] tarif { get; set; }
        public char[] status { get; set; }
        public char[] IBAN { get; set; }

        public char[] BIC { get; set; }
    }
}
