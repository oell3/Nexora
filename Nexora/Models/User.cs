namespace Nexora.Models
{
    public class User
    {
        public int? Id { get; set; }
        public int? Mandant_id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Vorname { get; set; }
        public string? Nachname { get; set; }
        public string? Rolle { get; set; }

        public bool? Is_owner { get; set; }
        public string? Status { get; set; }


    }
}
