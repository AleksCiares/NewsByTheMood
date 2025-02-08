using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsByTheMood.Data.Migrations
{
    /// <inheritdoc />
    public partial class make_foriegn_key_reqiured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Sources_SourceId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Topics_TopicId",
                table: "Sources");

            migrationBuilder.AlterColumn<long>(
                name: "TopicId",
                table: "Sources",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SourceId",
                table: "Articles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Sources_SourceId",
                table: "Articles",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Topics_TopicId",
                table: "Sources",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.Cascade);   
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Sources_SourceId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Topics_TopicId",
                table: "Sources");

            migrationBuilder.AlterColumn<long>(
                name: "TopicId",
                table: "Sources",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "SourceId",
                table: "Articles",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Sources_SourceId",
                table: "Articles",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Topics_TopicId",
                table: "Sources",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id");
        }
    }
}
