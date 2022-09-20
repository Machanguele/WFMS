using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class ComponentesStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectStatus");

            migrationBuilder.AddColumn<int>(
                name: "ComponentStatusId",
                table: "Components",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ComponentStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_ComponentStatusId",
                table: "Components",
                column: "ComponentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_ComponentStatus_ComponentStatusId",
                table: "Components",
                column: "ComponentStatusId",
                principalTable: "ComponentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_ComponentStatus_ComponentStatusId",
                table: "Components");

            migrationBuilder.DropTable(
                name: "ComponentStatus");

            migrationBuilder.DropIndex(
                name: "IX_Components_ComponentStatusId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ComponentStatusId",
                table: "Components");

            migrationBuilder.CreateTable(
                name: "ProjectStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStatus", x => x.Id);
                });
        }
    }
}
