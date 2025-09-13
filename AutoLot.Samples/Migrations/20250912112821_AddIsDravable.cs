using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLot.Samples.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDravable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Color",
                schema: "dbo",
                table: "Inventory",
                newName: "CarColor");

            migrationBuilder.AlterColumn<string>(
                name: "CarColor",
                schema: "dbo",
                table: "Inventory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Black",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "IsDravable",
                schema: "dbo",
                table: "Inventory",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventiry_MakeId",
                schema: "dbo",
                table: "Inventory",
                column: "MakeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inventiry_MakeId",
                schema: "dbo",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "IsDravable",
                schema: "dbo",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "CarColor",
                schema: "dbo",
                table: "Inventory",
                newName: "Color");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                schema: "dbo",
                table: "Inventory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Black");
        }
    }
}
