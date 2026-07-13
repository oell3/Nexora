namespace Nexora.Models
{
    public class Mandant
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Email { get; set; }

        public string? Telefon { get; set; }
        public string? Tarif { get; set; }
        public string? Status { get; set; }
        public string? IBAN { get; set; }

        public string? BIC { get; set; }
    }
}
