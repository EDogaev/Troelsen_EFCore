using AutoLot.Dal.Repos;
using AutoLot.Dal.Repos.Interfaces;
using AutoLot.Dal.Tests.Base;

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
}