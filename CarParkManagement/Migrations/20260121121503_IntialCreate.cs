using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarParkManagement.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    VehicleReg = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.VehicleReg);
                });

            migrationBuilder.CreateTable(
                name: "ParkedVehicles",
                columns: table => new
                {
                    SpaceNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkedVehicleVehicleReg = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParkingTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicles", x => x.SpaceNumber);
                    table.ForeignKey(
                        name: "FK_ParkedVehicles_Vehicle_ParkedVehicleVehicleReg",
                        column: x => x.ParkedVehicleVehicleReg,
                        principalTable: "Vehicle",
                        principalColumn: "VehicleReg");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_ParkedVehicleVehicleReg",
                table: "ParkedVehicles",
                column: "ParkedVehicleVehicleReg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkedVehicles");

            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
