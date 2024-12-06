using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration47 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailSegundo",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "EmailSocio",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "EmailTerceiro",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "InscricaoEstadual",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<bool>(
                name: "TipoPessoaSocioSegundo",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TipoPessoaSocioTerceiro",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSegundo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "EmailSocio",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "EmailTerceiro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "InscricaoEstadual",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "TipoPessoaSocioSegundo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "TipoPessoaSocioTerceiro",
                table: "Clientes");
        }
    }
}
