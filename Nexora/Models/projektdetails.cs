namespace Nexora.Models
{
    public class projektdetails
    {
        public int id { get; set; }
        public int projekt_id { get; set; }
        public float lon { get; set; }
        public float lat { get; set; }
        public string beschreibung { get; set; }
        public int objekt_id { get; set; }
        public DateTime datum { get; set; }
        public string name { get; set; }
        public int mandant_id { get; set; }

    }
}
