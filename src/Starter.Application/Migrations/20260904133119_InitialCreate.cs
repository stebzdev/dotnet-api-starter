using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Starter.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reporting_periods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportingEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceYear = table.Column<int>(type: "integer", nullable: false),
                    ReferenceMonth = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reporting_periods", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reporting_periods_ReportingEntityId_ReferenceYear_Reference~",
                table: "reporting_periods",
                columns: new[] { "ReportingEntityId", "ReferenceYear", "ReferenceMonth" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reporting_periods");
        }
    }
}
