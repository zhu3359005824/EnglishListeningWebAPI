using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Listening.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_albumName_atEpisode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_T_Episodes_AlbumId_IsDeleted",
                table: "T_Episodes");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "T_Episodes");

            migrationBuilder.AddColumn<string>(
                name: "AlbumName",
                table: "T_Episodes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_T_Episodes_AlbumName_IsDeleted",
                table: "T_Episodes",
                columns: new[] { "AlbumName", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_T_Episodes_AlbumName_IsDeleted",
                table: "T_Episodes");

            migrationBuilder.DropColumn(
                name: "AlbumName",
                table: "T_Episodes");

            migrationBuilder.AddColumn<Guid>(
                name: "AlbumId",
                table: "T_Episodes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_T_Episodes_AlbumId_IsDeleted",
                table: "T_Episodes",
                columns: new[] { "AlbumId", "IsDeleted" });
        }
    }
}
