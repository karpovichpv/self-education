using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DutchTreat.Migrations
{
    public partial class Identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2023, 8, 13, 19, 30, 24, 632, DateTimeKind.Utc).AddTicks(112));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2023, 8, 12, 19, 55, 20, 713, DateTimeKind.Utc).AddTicks(2828));
        }
    }
}
