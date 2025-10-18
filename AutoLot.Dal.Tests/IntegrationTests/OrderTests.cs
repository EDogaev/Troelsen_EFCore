using AutoLot.Dal.Repos;
using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Dal.Tests.Base;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Dal.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class OrderTests : BaseTest, IClassFixture<EnsureAutoLotDatabaseTestFixture>
{
    private readonly IOrderRepo _repo;

    public OrderTests()
    {
        _repo = new OrderRepo(Context);
    }

    public override void Dispose()
    {
        _repo.Dispose();
    }

    [Theory]
    [InlineData("Black", 2)]
    [InlineData("Rust", 1)]
    [InlineData("Yellow", 1)]
    [InlineData("Green", 0)]
    [InlineData("Pink", 1)]
    [InlineData("Brown", 0)]
    public void ShouldGetAllViewModelsByColor(string color, int expectedCount)
    {
        var query = _repo.GetOrdersViewModel()
            .Where(x => x.Color == color);
        var qs = query.ToQueryString();
        // qs = DECLARE @__color_0 nvarchar(4000) = N'Yellow';
        //      SELECT [c].[Color], [c].[FirstName], [c].[IsDravable], [c].[LastName], [c].[Make], [c].[PetName]
        //      FROM [dbo].[CustomerOrderView] AS [c]
        //      WHERE [c].[Color] = @__color_0
        var orders = query.ToList();
        Assert.Equal(expectedCount, orders.Count);
    }

    [Fact]
    public void ShouldGeAllOrdersExceptFiltered()
    {
        var query = Context.Orders.AsQueryable();
        var qs = query.ToQueryString();
        // qs = SELECT [o].[Id], [o].[CarId], [o].[CustomerId], [o].[TimeStamp]
        //      FROM [dbo].[Orders] AS [o]
        //      INNER JOIN (
        //          SELECT [i].[Id], [i].[IsDravable]
        //          FROM [dbo].[Inventory] AS [i]
        //          WHERE [i].[IsDravable] = CAST(1 AS bit)
        //      ) AS [i0] ON [o].[CarId] = [i0].[Id]
        //      WHERE [i0].[IsDravable] = CAST(1 AS bit)
        var orders = query.ToList();
        Assert.NotNull(orders);
        Assert.Equal(4, orders.Count);
    }
}