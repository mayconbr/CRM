using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration45 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChavePix",
                table: "Clientes");

            migrationBuilder.AddColumn<string>(
                name: "celularSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "celularSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "cpfSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "cpfSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "enderecoSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "enderecoSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "nomeSocioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "nomeSocioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "pepSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "pepTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "socioSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "socioTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "celularSocioSegundo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "celularSocioTerceiro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "cpfSocioSegundo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "cpfSocioTerceiro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "enderecoSocioSegundo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "enderecoSocioTerceiro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "nomeSocioSegundo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "nomeSocioTerceiro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "pepSegundo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "pepTerceiro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "socioSegundo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "socioTerceiro",
                table: "Clientes");

            migrationBuilder.AddColumn<string>(
                name: "ChavePix",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }
    }
}
