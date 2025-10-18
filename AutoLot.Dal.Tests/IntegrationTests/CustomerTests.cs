using System.Runtime.InteropServices;
using AutoLot.Dal.Tests.Base;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class CustomerTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    [Fact]
    public void ShouldGetAllTheCustomers()
    {
        var qs = Context.Customers.ToQueryString();
        // qs = SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
        //      FROM [dbo].[Customers] AS [c]
        var customers = Context.Customers.ToList();
        Assert.Equal(5, customers.Count);
    }

    [Fact]
    public void ShouldGetAllViewModels()
    {
        var qs = Context.Orders.ToQueryString();
        // qs = SELECT [o].[Id], [o].[CarId], [o].[CustomerId], [o].[TimeStamp]
        //      FROM [dbo].[Orders] AS [o]
        //      INNER JOIN (
        //          SELECT [i].[Id], [i].[IsDravable]
        //          FROM [dbo].[Inventory] AS [i]
        //          WHERE [i].[IsDravable] = CAST(1 AS bit)
        //      ) AS [i0] ON [o].[CarId] = [i0].[Id]
        //      WHERE [i0].[IsDravable] = CAST(1 AS bit)
        var orders = Context.Orders.ToList();
        Assert.NotEmpty(orders);
        Assert.Equal(5, orders.Count);
    }

    [Fact]
    public void ShouldGetCustomerWithLastNameW()
    {
        var query = Context.Customers.Where(x => x.PersonalInformation.LastName.StartsWith("W"));
        var qs = query.ToQueryString();
        // qs = SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
        //      FROM [dbo].[Customers] AS [c]
        //      WHERE [c].[LastName] LIKE N'W%'
        var customers = query.ToList();
        Assert.Equal(2, customers.Count);
    }

    [Fact]
    public void ShouldGetCustomerWithLastNameWAndFirstNameM()
    {
        var query = Context.Customers
            .Where(x => x.PersonalInformation.LastName.StartsWith("W"))
            .Where(x => x.PersonalInformation.FirstName.StartsWith("M"));
        // или
        //  .Where(x => x.PersonalInformation.LastName.StartsWith("W") && x.PersonalInformation.FirstName.StartsWith("M"));
        var qs = query.ToQueryString();
        // qs = SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
        //      FROM [dbo].[Customers] AS [c]
        //      WHERE [c].[LastName] LIKE N'W%' AND [c].[FirstName] LIKE N'M%'
        var customers = query.ToList();
        Assert.Single(customers);
    }

    [Fact]
    public void ShouldGetCustomersWithLastNameWOrH()
    {
        var query = Context.Customers
            .Where(x => x.PersonalInformation.LastName.StartsWith("W") ||
                        x.PersonalInformation.LastName.StartsWith("H"));
        // или
        //.Where(x => EF.Functions.Like(x.PersonalInformation.LastName, "W%") ||
        //            EF.Functions.Like(x.PersonalInformation.LastName, "H%"));
        var qs = query.ToQueryString();
        // qs = SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
        //      FROM [dbo].[Customers] AS [c]
        //      WHERE [c].[LastName] LIKE N'W%' OR [c].[LastName] LIKE N'H%'
        var customers = query.ToList();
        Assert.Equal(3, customers.Count);
    }

    [Fact]
    public void ShouldSortByLastNameThenFirstName()
    {
        // сортировать по фамилии, затем по имени
        var query = Context.Customers
            .OrderBy(x => x.PersonalInformation.LastName)
            .ThenBy(x => x.PersonalInformation.FirstName);
        var qs = query.ToQueryString();
        // qs = SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
        //      FROM [dbo].[Customers] AS [c]
        //      ORDER BY [c].[LastName], [c].[FirstName]

        var customers = query.ToList();

        if (customers.Count <= 1)
        {
            return;
        }

        for (int i = 0; i < customers.Count - 1; i++)
        {
            var pi = customers[i].PersonalInformation;
            var pi2 = customers[i + 1].PersonalInformation;
            var compareLastName = string.Compare(pi.LastName, pi2.LastName, StringComparison.CurrentCultureIgnoreCase);
            Assert.True(compareLastName <= 0);

            if (compareLastName != 0)
            {
                continue;
            }

            var compareFirstName =
                string.Compare(pi.FirstName, pi2.FirstName, StringComparison.CurrentCultureIgnoreCase);
            Assert.True(compareFirstName <= 0);
        }
    }

    [Fact]
    public void ShouldSortByLastNameThenFirstNameUsingReverse()
    {
        // сортировать по фамилии, затем по имени
        // и изменить порядок сортировки на противоположный
        var query = Context.Customers
            .OrderBy(x => x.PersonalInformation.LastName)
            .ThenBy(x => x.PersonalInformation.FirstName)
            .Reverse();
        var qs = query.ToQueryString();
        // qs = SELECT [c].[Id], [c].[TimeStamp], [c].[FirstName], [c].[FullName], [c].[LastName]
        //      FROM [dbo].[Customers] AS [c]
        //      ORDER BY [c].[LastName] DESC, [c].[FirstName] DESC
        var customers = query.ToList();

        if (customers.Count <= 1)
        {
            return;
        }

        for (int i = 0; i < customers.Count - 1; i++)
        {
            var pi = customers[i].PersonalInformation;
            var pi2 = customers[i + 1].PersonalInformation;
            var compareLastName = string.Compare(pi.LastName, pi2.LastName, StringComparison.CurrentCultureIgnoreCase);
            Assert.True(compareLastName >= 0);

            if (compareLastName != 0)
            {
                continue;
            }

            var compareFirstName =
                string.Compare(pi.FirstName, pi2.FirstName, StringComparison.CurrentCultureIgnoreCase);
            Assert.True(compareFirstName >= 0);
        }
    }

    [Fact]
    public void GetFirstMatchingRecordDatabaseOrder()
    {
        // Получить первую запись на основе порядка в базе данных.
        var customer = Context.Customers.First();
        // SELECT ТОР(1) [с].[Id], [с].[TimeStamp],
        // [с].[FirstName], [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
        Assert.Equal(1, customer.Id);
    }

    [Fact]
    public void GetFirstMatchingRecordNameOrder()
    {
        // Получить первую запись на основе порядка "фамилия, имя",
        var customer = Context.Customers
            .OrderBy(x => x.PersonalInformation.LastName)
            .ThenBy(x => x.PersonalInformation.FirstName)
            .First();
        // SELECT Т0Р(1) [с].[Id], [с].[TimeStamp],
        // [с].[FirstName], [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
        // ORDER BY [c].[LastName], [c].[FirstName]
        Assert.Equal(1, customer.Id);
    }

    [Fact]
    public void FirstShouldThrowExceptionIfNoneMatch()
    {
        // Фильтровать на основе Id.
        // Сгенерировать исключение, если соответствие не найдено.
        Assert.Throws<InvalidOperationException>(() =>
            Context.Customers.First(x => x.Id == 10));
        // SELECT ТОР(1) [с].[Id], [с].[TimeStamp],
        // [с].[FirstName], [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
        // WHERE [c].[Id] = 10
    }
    [Fact]
    public void FirstOrDefaultShouldThrowExceptionIfNoneMatch()
    {
        // Возвращает null, если ничего не найдено.
        var customer = Context.Customers.FirstOrDefault(x => x.Id == 10);
        // SELECT ТОР(1) [с].[Id], [с].[TimeStamp],
        // [с].[FirstName], [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
        // WHERE [c].[Id] = 10
        Assert.Null(customer);
    }

    [Fact]
    public void GetLastMatchingRecordNameOrder()
    {
        // Получить первую запись на основе порядка "фамилия, имя",
        var customer = Context.Customers
            .OrderBy(x => x.PersonalInformation.LastName)
            .ThenBy(x => x.PersonalInformation.FirstName)
            .Last();
        // SELECT Т0Р(1) [с].[Id], [с].[TimeStamp],
        // [с].[FirstName], [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
        // ORDER BY [c].[LastName] DESC, [c].[FirstName] DESC
        Assert.Equal(4, customer.Id);
    }

    // Концептуально Single()/SingleOrDefault() работает аналогично First()/FirstOrDefault().
    // Основное отличие в том, что метод Single()/SingleOrDefault() возвращает TOP(2), а не ТОР(1),
    // и генерирует исключение, если из базы данных возвращаются две записи.
    [Fact]
    public void GetOneMatchingRecordWithSingle()
    {
        // получить первую запись на основе порядка в базе данных
        var customer = Context.Customers.Single(x => x.Id == 1);
        // SELECT Т0Р(2) [с].[Id], [с].[TimeStamp],
        // [с].[FirstName], [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
        // WHERE [c].[Id] = 1
        Assert.Equal(1, customer.Id);
    }

    [Fact]
    public void SingleShouldThrowExceptionIfNoneMathcing()
    {
        // фильтровать на основе Id.
        // сгенерировать исключение, если соответствие не найдено
        Assert.Throws<InvalidOperationException>(() =>
            Context.Customers.Single(x => x.Id == 10));
        // SELECT TOP(2) [с].[Id], [с].[TimeStamp],
        // [с].[FirstName], [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
        // WHERE [c].[Id] = 10
    }

    // Если при использовании Single() или SingleOrDefault() возвращается больше чем одна запись, тогда генерируется исключение:
    [Fact]
    public void SingleShouldThrowExceptionlfMoreThenOneMatch()
    {
        // Сгенерировать исключение, если найдено более одного соответствия.
        Assert.Throws<InvalidOperationException>(()
            => Context.Customers.Single());
        // SELECT TOP(2) [с].[Id], [с].[TimeStamp],
        // [с].[FirstName], [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
    }

    [Fact]
    public void SingleOrDefaultShouldThrowExceptionlfMoreThenOneMatch()
    {
        // Сгенерировать исключение, если найдено более одного соответствия.
        Assert.Throws<InvalidOperationException>(()
            => Context.Customers.SingleOrDefault());
        // SELECT TOP(2) [с].[Id], [с].[TimeStamp],
        // [с].[FirstName], [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
    }

    // Если никакие данные не возвращаются в случае применения SingleOrDefault(), то результатом будет null, а не исключение:
    [Fact]
    public void SingleOrDefaultShouldReturnDefaultlfNoneMatch()
    {
        var customer = Context.Customers.SingleOrDefault(x => x.Id == 11);
        Assert.Null(customer);
        // SELECT TOP(1) [с].[Id], [с].[TimeStamp], [с].[FirstName],
        // [с].[FullName], [с].[LastName]
        // FROM [Dbo].[Customers] AS [c]
        // WHERE [c].[Id] = 1 0
    }
}