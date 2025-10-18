using AutoLot.Dal.Tests.Base;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Dal.Tests.IntegrationTests;

// В зависимости от возможностей средства запуска тестов тесты xUnit выполняются
// последовательно внутри тестовой оснастки (класса), но параллельно во всех тестовых оснастках (классах).
// Это может оказаться проблематичным при прогоне интеграционных тестов, взаимодействующих с единственной базой данных.
// Выполнение можно сделать последовательным для всех тестовых оснасток, добавив их в одну и ту же тестовую коллекцию.
// Тестовые коллекции определяются по имени с применением атрибута [Collection] к классу.

[Collection("Integration Tests")]
public class CarTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(6, 1)]
    public void ShouldCarsByMake(int makeId, int expectedCount)
    {
        var query = Context.Cars
            .Where(x => x.MakeId == makeId);
        var qs = query.ToQueryString();
        // qs = DECLARE @__makeId_0 int = 1;
        //      SELECT [i].[Id], [i].[Color], [i].[IsDravable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
        //      FROM [dbo].[Inventory] AS [i]
        //      WHERE [i].[IsDravable] = CAST(1 AS bit) AND [i].[MakeId] = @__makeId_0
        var cars = query.ToList();
        Assert.Equal(expectedCount, cars.Count);
    }

    [Fact]
    public void ShouldReturnDrivableCarsWithQueryFilterSet()
    {
        var query = Context.Cars;
        var qs = query.ToQueryString();
        // qs = SELECT [i].[Id], [i].[Color], [i].[IsDravable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
        //      FROM [dbo].[Inventory] AS [i]
        //      WHERE [i].[IsDravable] = CAST(1 AS bit)
        var cars = query.ToList();
        Assert.NotEmpty(cars);
        Assert.Equal(9, cars.Count);
    }

    [Fact]
    public void ShouldGetAllOfTheCars()
    {
        var query = Context.Cars.IgnoreQueryFilters();
        var qs = query.ToQueryString();
        // qs = SELECT [i].[Id], [i].[Color], [i].[IsDravable], [i].[MakeId], [i].[PetName], [i].[TimeStamp]
        //      FROM [dbo].[Inventory] AS [i]
        var cars = query.ToList();
        Assert.Equal(10, cars.Count);
    }

    // Когда методы Include()/Thenlnclude() транслируются в SQL,
    // для обязательных отношений применяется внутреннее соединение,
    // а для необязательных — левое соединение.
    // Тест добавляет к результатам свойство MakeNavigation, выполняя внутреннее соединение.
    [Fact]
    public void ShouldGetAllOfTheCarsWithMakes()
    {
        var query = Context.Cars.Include(c => c.MakeNavigation);
        var qs = query.ToQueryString();
        // qs = SELECT [i].[Id], [i].[Color], [i].[IsDravable], [i].[MakeId], [i].[PetName], [i].[TimeStamp],
        //             [m].[Id], [m].[Name], [m].[TimeStamp]
        //      FROM [dbo].[Inventory] AS [i]
        //      INNER JOIN [dbo].[Makes] AS [m] ON [i].[MakeId] = [m].[Id]
        //      WHERE [i].[IsDravable] = CAST(1 AS bit)
        var cars = query.ToList();
        Assert.Equal(9, cars.Count);
    }

    // Во втором тесте используется два набора связанных данных.
    // Первый — это получение информации Маке (как и в предыдущем тесте),
    // а второй — получение сущностей Order и затем присоединенных к ним сущностей Customer.
    // Полный тест также отфильтровывает записи Саг, для которых есть записи Order.
    // Для необязательных отношений генерируются левые соединения:
    [Fact]
    public void ShouldGetCarsOnOrderWithRelatedProperties()
    {
        var query = Context.Cars
            .Where(c => c.Orders.Any())
            .Include(c => c.MakeNavigation)
            .Include(c => c.Orders).ThenInclude(o => o.CustomerNavigation);
        var qs = query.ToQueryString();
        var cars = query.ToList();
        Assert.Equal(4, cars.Count);
    }
}