using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "carteiradeCredito",
                table: "endividamentoSCR",
                newName: "carteiraVencer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "carteiraVencer",
                table: "endividamentoSCR",
                newName: "carteiradeCredito");
        }
    }
}
