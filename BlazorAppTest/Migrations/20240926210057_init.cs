using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAppTest.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cprs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CprNumber = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cprs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Todolists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    task = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CprId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todolists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Todolists_Cprs_CprId",
                        column: x => x.CprId,
                        principalTable: "Cprs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todolists_CprId",
                table: "Todolists",
                column: "CprId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todolists");

            migrationBuilder.DropTable(
                name: "Cprs");
        }
    }
}
