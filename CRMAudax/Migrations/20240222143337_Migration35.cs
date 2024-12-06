using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration35 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataDelete",
                table: "Pastas",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataDelete",
                table: "Pastas");
        }
    }
}
