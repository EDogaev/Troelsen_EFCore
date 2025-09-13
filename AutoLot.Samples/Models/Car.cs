using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Samples.Models;

[Table("Inventory", Schema = "dbo")]
[Index(nameof(MakeId), Name = "IX_Inventory_MakeId")]
public class Car : BaseEntity
{
    private bool? _isDravable;

    [Required, StringLength(50)]
    public string Color { get; set; }

    [Required, StringLength(50)]
    public string PetName { get; set; }

    // Проблема возникает при наличии стандартного значения у типа данных свойства.
    // Вспомните, что стандартное значение для числовых типов составляет 0, а для булевских — false. Если вы установите значение числового свойства в 0 или булевского
    // свойства в false и затем вставите такую сущность, тогда инфраструктура EF Core будет трактовать это свойство как не имеющее установленного значения.
    // При отображении свойства на столбец со стандартным значением используется стандартное значение в определении столбца.
    // Если вы модифицируете IsDrivable с целью применения поддерживающего поля, допускающего null (но оставите свойство не допускающим null), то EF Core
    // будет выполнять чтение и запись, используя поддерживающее поле, а не свойство. Стандартным значением для булевского типа, допускающего null, является null — не false.
    public bool IsDravable
    {
        get => _isDravable ?? true;
        set => _isDravable = value;
    }

    public int MakeId { get; set; }
    [ForeignKey(nameof(MakeId))]
    public Make MakeNavigation { get; set; }

    public Radio RadioNavigation { get; set; }

    [InverseProperty(nameof(Driver.Cars))]
    public IEnumerable<Driver> Drivers { get; set; } = new List<Driver>();
}