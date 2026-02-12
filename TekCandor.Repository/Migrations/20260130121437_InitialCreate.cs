using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekCandor.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClearingStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearingStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cycles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hub",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Code = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    CrAccSameDay = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    CrAccNormal = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    CrAccIntercity = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    CrAccDollar = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hub", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnReason",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AlphaReturnCodes = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NumericReturnCodes = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DescriptionWithReturnCodes = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    DefaultCallBack = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RevokedTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevokedTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    NIFTBranchCode = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    Email1 = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Email2 = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Email3 = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_Hub_HubId",
                        column: x => x.HubId,
                        principalTable: "Hub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    LoginName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PasswordHash = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    BranchorHub = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UserType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    HubIds = table.Column<long>(type: "bigint", nullable: true),
                    BranchIds = table.Column<long>(type: "bigint", nullable: true),
                    PhoneNo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PasswordLastChanged = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastLoginTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsSupervise = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Branch_BranchIds",
                        column: x => x.BranchIds,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Hub_HubIds",
                        column: x => x.HubIds,
                        principalTable: "Hub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branch_HubId",
                table: "Branch",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BranchIds",
                table: "Users",
                column: "BranchIds");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HubIds",
                table: "Users",
                column: "HubIds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "ClearingStatuses");

            migrationBuilder.DropTable(
                name: "Cycles");

            migrationBuilder.DropTable(
                name: "ReturnReason");

            migrationBuilder.DropTable(
                name: "RevokedTokens");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Hub");
        }
    }
}
