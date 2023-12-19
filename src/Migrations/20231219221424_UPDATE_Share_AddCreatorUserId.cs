using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaExchange.API.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_Share_AddCreatorUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorUserId",
                table: "Shares",
                type: "character varying(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shares_CreatorUserId",
                table: "Shares",
                column: "CreatorUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shares_CreatorUserId",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Shares");
        }
    }
}
