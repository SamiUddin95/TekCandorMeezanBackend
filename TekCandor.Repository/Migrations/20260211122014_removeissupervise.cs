using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekCandor.Repository.Migrations
{
    /// <inheritdoc />
    public partial class removeissupervise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSupervise",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSupervise",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
