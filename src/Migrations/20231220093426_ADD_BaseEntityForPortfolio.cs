using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaExchange.API.Migrations
{
    /// <inheritdoc />
    public partial class ADD_BaseEntityForPortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserShares",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserShares",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserShares");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserShares");
        }
    }
}
