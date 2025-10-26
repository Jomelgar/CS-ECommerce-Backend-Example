using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models;

public class Inventory
{
    [Key]
    public int id { get; set; }

    [ForeignKey(nameof(Product))]
    public int id_product { get; set; }
    public int total_amount { get; set; } = 0;

    /*
    public Product? Product { get; set; }

    public ICollection<Order>? Orders { get; set; }
    */
}