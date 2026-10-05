using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Starter.Application.Migrations;

/// <inheritdoc />
public partial class AddReferencePeriod : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // ReferencePeriod reuses the existing ReferenceYear and ReferenceMonth
        // columns, so no database schema changes are required.
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // ReferencePeriod did not change the database schema,
        // so there are no schema changes to revert.
    }
}
