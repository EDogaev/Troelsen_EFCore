using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLot.Models.Entities;

public partial class Customer
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public byte[]? TimeStamp { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<CreditRisk> CreditRisks { get; set; } = new List<CreditRisk>();

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
