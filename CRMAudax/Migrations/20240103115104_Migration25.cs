using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "carteiraVencer",
                table: "endividamentoSCR",
                newName: "carteiradeCredito");

            migrationBuilder.AddColumn<string>(
                name: "carteiraVencido",
                table: "endividamentoSCR",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "carteiraVencido",
                table: "endividamentoSCR");

            migrationBuilder.RenameColumn(
                name: "carteiradeCredito",
                table: "endividamentoSCR",
                newName: "carteiraVencer");
        }
    }
}
