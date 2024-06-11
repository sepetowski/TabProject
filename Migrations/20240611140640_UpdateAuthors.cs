using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TabProjectServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuthors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookDescripton",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookDescripton",
                table: "Books");
        }
    }
}
