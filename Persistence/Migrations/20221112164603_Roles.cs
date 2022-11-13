using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationRolePermissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationRolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationPermissionId = table.Column<int>(type: "int", nullable: true),
                    ApplicationRoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationRolePermissions_ApplicationPermissions_ApplicationPermissionId",
                        column: x => x.ApplicationPermissionId,
                        principalTable: "ApplicationPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationRolePermissions_ApplicationRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "ApplicationRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRolePermissions_ApplicationPermissionId",
                table: "ApplicationRolePermissions",
                column: "ApplicationPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRolePermissions_ApplicationRoleId",
                table: "ApplicationRolePermissions",
                column: "ApplicationRoleId");
        }
    }
}
