using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TieredEntity.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlDutyPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    NextId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlDutyPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlJobRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    NextId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlJobRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NsDutyPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TreeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Left = table.Column<int>(type: "INTEGER", nullable: false),
                    Right = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NsDutyPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NsJobRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TreeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Left = table.Column<int>(type: "INTEGER", nullable: false),
                    Right = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NsJobRoles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlDutyPositions");

            migrationBuilder.DropTable(
                name: "AlJobRoles");

            migrationBuilder.DropTable(
                name: "NsDutyPositions");

            migrationBuilder.DropTable(
                name: "NsJobRoles");
        }
    }
}
