using AutoLot.Models.Entities.Base;
using AutoLot.Models.Entities.Owned;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLot.Models.Entities;

public class CreditRisk : BaseEntity
{
    public Person PersonalInformation { get; set; } = new();

    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    [InverseProperty(nameof(Customer.CreditRisks))]
    public Customer? CustomerNavigation { get; set; }
}
