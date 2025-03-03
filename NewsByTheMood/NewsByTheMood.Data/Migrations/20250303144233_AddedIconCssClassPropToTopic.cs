using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsByTheMood.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIconCssClassPropToTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconCssClass",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconCssClass",
                table: "Topics");
        }
    }
}
