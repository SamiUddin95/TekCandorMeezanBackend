using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekCandor.Repository.Migrations
{
    /// <inheritdoc />
    public partial class modifyPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Group_GroupId",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_GroupId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Permission");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "Permission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_GroupId",
                table: "Permission",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Group_GroupId",
                table: "Permission",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
