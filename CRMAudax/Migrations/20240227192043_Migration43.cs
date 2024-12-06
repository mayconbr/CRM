using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ArquivoId",
                table: "CompartilhaArquivos",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompartilhaArquivos_ArquivoId",
                table: "CompartilhaArquivos",
                column: "ArquivoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompartilhaArquivos_ArquivosPastas_ArquivoId",
                table: "CompartilhaArquivos",
                column: "ArquivoId",
                principalTable: "ArquivosPastas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompartilhaArquivos_ArquivosPastas_ArquivoId",
                table: "CompartilhaArquivos");

            migrationBuilder.DropIndex(
                name: "IX_CompartilhaArquivos_ArquivoId",
                table: "CompartilhaArquivos");

            migrationBuilder.DropColumn(
                name: "ArquivoId",
                table: "CompartilhaArquivos");
        }
    }
}
