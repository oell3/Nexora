namespace Nexora.Models
{
    public class User
    {
        public int Id { get; set; }
        public int mandant_id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public char vorname { get; set; }
        public char nachname { get; set; }
        public char rolle { get; set; }

        public bool is_owner { get; set; }
        public char status { get; set; }


    }
}
