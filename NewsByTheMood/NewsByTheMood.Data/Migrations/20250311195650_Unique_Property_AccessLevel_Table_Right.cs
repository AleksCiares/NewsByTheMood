using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsByTheMood.Data.Migrations
{
    /// <inheritdoc />
    public partial class Unique_Property_AccessLevel_Table_Right : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccessLevel",
                table: "Rights",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Rights_AccessLevel",
                table: "Rights",
                column: "AccessLevel",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rights_AccessLevel",
                table: "Rights");

            migrationBuilder.AlterColumn<string>(
                name: "AccessLevel",
                table: "Rights",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
