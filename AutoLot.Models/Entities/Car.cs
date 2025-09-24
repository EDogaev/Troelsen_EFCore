using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AutoLot.Models.Entities.Base;

namespace AutoLot.Models.Entities;

[Table("Inventory", Schema = "dbo")]
[Index(nameof(MakeId), Name = "IX_Inventory_MakeId")]
public class Car : BaseEntity
{
    private bool? _isDravable;

    [Required]
    [DisplayName("Make")]
    public int MakeId { get; set; }

    [Required]
    [StringLength(50)]
    public string Color { get; set; } = "Gold";

    [Required]
    [StringLength(50)]
    [DisplayName("Pet Name")]
    public string PetName { get; set; } = "My Precious";

    [ForeignKey(nameof(MakeId))]
    [InverseProperty(nameof(Make.Cars))]
    public Make? MakeNavigation { get; set; }

    [JsonIgnore]
    [InverseProperty(nameof(Order.CarNavigation))]
    public IEnumerable<Order> Orders { get; set; } = new List<Order>();

    [NotMapped]
    public string MakeName => MakeNavigation?.Name ?? "Unknown";

    [DisplayName("Is Dravable")]
    public bool IsDravable
    {
        get => _isDravable ?? false;
        set => _isDravable = value;
    }

    public override string ToString()
    {
        return $"{PetName} is a {Color} {MakeNavigation?.Name} with ID {Id}";
    }
}
