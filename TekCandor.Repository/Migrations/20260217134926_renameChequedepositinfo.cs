using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekCandor.Repository.Migrations
{
    /// <inheritdoc />
    public partial class renameChequedepositinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_chequedepositInfos",
                table: "chequedepositInfos");

            migrationBuilder.RenameTable(
                name: "chequedepositInfos",
                newName: "chequedepositInformation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chequedepositInformation",
                table: "chequedepositInformation",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_chequedepositInformation",
                table: "chequedepositInformation");

            migrationBuilder.RenameTable(
                name: "chequedepositInformation",
                newName: "chequedepositInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chequedepositInfos",
                table: "chequedepositInfos",
                column: "Id");
        }
    }
}
