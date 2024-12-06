using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration44 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChavePix",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "contaBancaria",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChavePix",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "contaBancaria",
                table: "Clientes");
        }
    }
}
