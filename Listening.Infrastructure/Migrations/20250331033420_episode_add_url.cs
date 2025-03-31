using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Listening.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class episode_add_url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AudioUrl",
                table: "T_Episodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioUrl",
                table: "T_Episodes");
        }
    }
}
