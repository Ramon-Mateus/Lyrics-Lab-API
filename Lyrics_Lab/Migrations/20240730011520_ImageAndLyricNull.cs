using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lyrics_Lab.Migrations
{
    /// <inheritdoc />
    public partial class ImageAndLyricNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Playlists",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Playlists");
        }
    }
}
