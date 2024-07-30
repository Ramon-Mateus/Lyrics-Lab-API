using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lyrics_Lab.Migrations
{
    /// <inheritdoc />
    public partial class AddToneToSongs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tom",
                table: "Songs",
                newName: "Tone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tone",
                table: "Songs",
                newName: "Tom");
        }
    }
}
