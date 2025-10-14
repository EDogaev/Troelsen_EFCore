using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Models.ViewModels;

// Аннотация данных [KeyLess] указывает, что класс является сущностью, работающей с данными,
// которые не имеют первичного ключа и могут быть оптимизированы
// как данные только для чтения (с точки зрения базы данных). 
[Keyless]
public class CustomerOrderViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Color { get; set; }
    public string? PetName { get; set; }
    public string? Make { get; set; }
    public bool? IsDaravable { get; set; }
    [NotMapped]
    public string FullDetail => $"{FirstName} {LastName} ordered a {Color} {Make} named {PetName}";

    public override string ToString() => FullDetail;
}