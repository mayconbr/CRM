using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dominio",
                table: "endividamentoSCR");

            migrationBuilder.DropColumn(
                name: "Modalidade",
                table: "endividamentoSCR");

            migrationBuilder.DropColumn(
                name: "Subdominio",
                table: "endividamentoSCR");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "endividamentoSCR");

            migrationBuilder.RenameColumn(
                name: "ValorVencimento",
                table: "endividamentoSCR",
                newName: "carteiraVencer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "carteiraVencer",
                table: "endividamentoSCR",
                newName: "ValorVencimento");

            migrationBuilder.AddColumn<string>(
                name: "Dominio",
                table: "endividamentoSCR",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "Modalidade",
                table: "endividamentoSCR",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "Subdominio",
                table: "endividamentoSCR",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "endividamentoSCR",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }
    }
}
