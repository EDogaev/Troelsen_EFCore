using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLot.Models.Entities;

[Table("Inventory")]
[Index("MakeId", Name = "IX_Inventory_MakeId")]
public partial class Inventory
{
    [Key]
    public int Id { get; set; }

    public int MakeId { get; set; }

    [StringLength(50)]
    public string Color { get; set; } = null!;

    [StringLength(50)]
    public string PetName { get; set; } = null!;

    public byte[]? TimeStamp { get; set; }

    [ForeignKey("MakeId")]
    [InverseProperty("Inventories")]
    public virtual Make Make { get; set; } = null!;

    [InverseProperty("Car")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
