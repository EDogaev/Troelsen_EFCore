using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoLot.Dal.EfStructures;

public static class MigrationHelpers
{
    public static void CreateSproc(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            exec (N'
            CREATE PROCEDURE [dbo].[GetPetName]
                @carId int,
                @petName nvarchar(50) output
            AS
            SELECT @petName = PetName
            FROM dbo.Inventory
            WHERE Id = @carId
        ')");
    }

    public static void DropSproc(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetPetName]");
    }

    public static void CreateCustomerOrderView(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            exec (N'
            CREATE VIEW [dbo].[CustomerOrderView]
            AS
            SELECT dbo.Customers.FirstName, dbo.Customers.LastName,
                dbo.Inventory.Color, dbo.Inventory.PetName, 
                dbo.Inventory.IsDravable, dbo.Makes.Name AS Make
            FROM dbo.Orders
            INNER JOIN dbo.Customers ON dbo.Orders.CustomerId = dbo.Customers.Id
            INNER JOIN dbo.Inventory ON dbo.Orders.CarId = dbo.Inventory.Id
            INNER JOIN dbo.Makes ON dbo.Makes.Id = dbo.Inventory.Id
        ')");
    }

    public static void DropCustomerOrderView(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP VIEW [dbo].[CustomerOrderView]");
    }
}