using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LyricsWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SongItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Album = table.Column<string>(type: "TEXT", nullable: false),
                    Artist = table.Column<string>(type: "TEXT", nullable: false),
                    Lyrics = table.Column<string>(type: "TEXT", nullable: false),
                    CoverPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongItems", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongItems");
        }
    }
}
