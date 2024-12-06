using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration55 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssinaturaSocio",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "AssinaturaSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "AssinaturaSocioTerceiro",
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
                name: "AssinaturaSocio",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "AssinaturaSocioSegundo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "AssinaturaSocioTerceiro",
                table: "Clientes");
        }
    }
}
