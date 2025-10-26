namespace Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order
    {
        [Key]
        public int id { get; set; } 
        public int amount { get; set; }
        [ForeignKey("Inventory")]
        public int id_inventory { get; set; }
        /*
        public Inventory? Inventory { get; set; }
        */

    }
}