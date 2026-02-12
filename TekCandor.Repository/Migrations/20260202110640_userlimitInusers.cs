using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekCandor.Repository.Migrations
{
    /// <inheritdoc />
    public partial class userlimitInusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserLimit",
                table: "Users",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLimit",
                table: "Users");
        }
    }
}
