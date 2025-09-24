using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLot.Models.Entities.Owned;

[Owned]
public class Person
{
    [Required, StringLength(50)]
    public string FirstName { get; set; } = "New";

    [Required, StringLength(50)]
    public string LastName { get; set; } = "CustomerNavigation";

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string? FullName { get; set; }
}