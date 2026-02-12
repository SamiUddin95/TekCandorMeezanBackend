using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekCandor.Repository.Migrations
{
    /// <inheritdoc />
    public partial class deleteForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Branch_BranchIds",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Hub_HubIds",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BranchIds",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_HubIds",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "HubIds",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BranchIds",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "HubIds",
                table: "Users",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BranchIds",
                table: "Users",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BranchIds",
                table: "Users",
                column: "BranchIds");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HubIds",
                table: "Users",
                column: "HubIds");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Branch_BranchIds",
                table: "Users",
                column: "BranchIds",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Hub_HubIds",
                table: "Users",
                column: "HubIds",
                principalTable: "Hub",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
