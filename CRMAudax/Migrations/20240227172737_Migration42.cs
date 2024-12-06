using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration42 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PastaId",
                table: "CompartilhaArquivos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PastasId",
                table: "CompartilhaArquivos",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompartilhaArquivos_PastasId",
                table: "CompartilhaArquivos",
                column: "PastasId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompartilhaArquivos_Pastas_PastasId",
                table: "CompartilhaArquivos",
                column: "PastasId",
                principalTable: "Pastas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompartilhaArquivos_Pastas_PastasId",
                table: "CompartilhaArquivos");

            migrationBuilder.DropIndex(
                name: "IX_CompartilhaArquivos_PastasId",
                table: "CompartilhaArquivos");

            migrationBuilder.DropColumn(
                name: "PastaId",
                table: "CompartilhaArquivos");

            migrationBuilder.DropColumn(
                name: "PastasId",
                table: "CompartilhaArquivos");
        }
    }
}
