using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Starter.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddReportingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reporting_entities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reporting_entities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reporting_entities_Code",
                table: "reporting_entities",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_reporting_periods_reporting_entities_ReportingEntityId",
                table: "reporting_periods",
                column: "ReportingEntityId",
                principalTable: "reporting_entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reporting_periods_reporting_entities_ReportingEntityId",
                table: "reporting_periods");

            migrationBuilder.DropTable(
                name: "reporting_entities");
        }
    }
}
