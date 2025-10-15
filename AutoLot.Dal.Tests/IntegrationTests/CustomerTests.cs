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
}