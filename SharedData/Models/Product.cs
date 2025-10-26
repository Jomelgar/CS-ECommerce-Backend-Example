namespace SharedData.Models{
    using System.ComponentModel.DataAnnotations;
    public class Product
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; } = 0;
        /*public Inventory? Inventory { get; set; }*/
    }
}