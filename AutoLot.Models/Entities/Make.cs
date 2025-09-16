using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLot.Models.Entities;

public partial class Make
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public byte[]? TimeStamp { get; set; }

    [InverseProperty("Make")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
