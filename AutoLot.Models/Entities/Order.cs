using AutoLot.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLot.Models.Entities;

[Table("Orders", Schema = "dbo")]
[Index(nameof(CarId), Name = "IX_Orders_CarId")]
[Index(nameof(CustomerId), nameof(CarId), Name = "IX_Orders_CustomerId_CarId", IsUnique = true)]
public class Order : BaseEntity
{
    public int CustomerId { get; set; }

    public int CarId { get; set; }

    [ForeignKey("CarId")]
    [InverseProperty(nameof(Car.Orders))]
    public virtual Car? CarNavigation { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty(nameof(Customer.Orders))]
    public virtual Customer? CustomerNavigation { get; set; } = null!;
}
