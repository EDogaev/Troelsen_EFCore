using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLot.Dal.EfStructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Make_Inventory",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers",
                table: "Orders");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "Logging");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Orders",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Makes",
                newName: "Makes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Inventory",
                newName: "Inventory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customers",
                newSchema: "dbo");

            migrationBuilder.AddColumn<bool>(
                name: "IsDravable",
                schema: "dbo",
                table: "Inventory",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "CreditRisks",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[LastName] + ', ' + [FirstName]");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "dbo",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[LastName] + ', ' + [FirstName]");

            migrationBuilder.CreateTable(
                name: "SeriLogs",
                schema: "Logging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GetDate()"),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "Xml", nullable: true),
                    LogEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MashineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriLogs", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Make_Inventory",
                schema: "dbo",
                table: "Inventory",
                column: "MakeId",
                principalSchema: "dbo",
                principalTable: "Makes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers",
                schema: "dbo",
                table: "Orders",
                column: "CustomerId",
                principalSchema: "dbo",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Make_Inventory",
                schema: "dbo",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "SeriLogs",
                schema: "Logging");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "CreditRisks");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "dbo",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsDravable",
                schema: "dbo",
                table: "Inventory");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "dbo",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "Makes",
                schema: "dbo",
                newName: "Makes");

            migrationBuilder.RenameTable(
                name: "Inventory",
                schema: "dbo",
                newName: "Inventory");

            migrationBuilder.RenameTable(
                name: "Customers",
                schema: "dbo",
                newName: "Customers");

            migrationBuilder.AddForeignKey(
                name: "FK_Make_Inventory",
                table: "Inventory",
                column: "MakeId",
                principalTable: "Makes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
