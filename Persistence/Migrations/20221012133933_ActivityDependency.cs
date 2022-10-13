using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class ActivityDependency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityDependencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainActivityId = table.Column<int>(type: "int", nullable: true),
                    DependencyActivityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityDependencies_Activities_DependencyActivityId",
                        column: x => x.DependencyActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityDependencies_Activities_MainActivityId",
                        column: x => x.MainActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDependencies_DependencyActivityId",
                table: "ActivityDependencies",
                column: "DependencyActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDependencies_MainActivityId",
                table: "ActivityDependencies",
                column: "MainActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDependencies");
        }
    }
}
