using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLot.Samples.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureByModelCreating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDriver_Drivers_DriversId",
                table: "CarDriver");

            migrationBuilder.DropForeignKey(
                name: "FK_CarDriver_Inventory_CarsId",
                table: "CarDriver");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Makes_MakeId",
                schema: "dbo",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "DriversId",
                table: "CarDriver",
                newName: "DriverID");

            migrationBuilder.RenameColumn(
                name: "CarsId",
                table: "CarDriver",
                newName: "CarId");

            migrationBuilder.RenameIndex(
                name: "IX_CarDriver_DriversId",
                table: "CarDriver",
                newName: "IX_CarDriver_DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDriver_Cars_CarId",
                table: "CarDriver",
                column: "CarId",
                principalSchema: "dbo",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarDriver_Drivers_DriverId",
                table: "CarDriver",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Makes_MakeId",
                schema: "dbo",
                table: "Inventory",
                column: "MakeId",
                principalTable: "Makes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDriver_Cars_CarId",
                table: "CarDriver");

            migrationBuilder.DropForeignKey(
                name: "FK_CarDriver_Drivers_DriverId",
                table: "CarDriver");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Makes_MakeId",
                schema: "dbo",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "DriverID",
                table: "CarDriver",
                newName: "DriversId");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "CarDriver",
                newName: "CarsId");

            migrationBuilder.RenameIndex(
                name: "IX_CarDriver_DriverID",
                table: "CarDriver",
                newName: "IX_CarDriver_DriversId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventiry_MakeId",
                schema: "dbo",
                table: "Inventory",
                column: "MakeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDriver_Drivers_DriversId",
                table: "CarDriver",
                column: "DriversId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarDriver_Inventory_CarsId",
                table: "CarDriver",
                column: "CarsId",
                principalSchema: "dbo",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Makes_MakeId",
                schema: "dbo",
                table: "Inventory",
                column: "MakeId",
                principalTable: "Makes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
