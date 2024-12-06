using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RegiaoId",
                table: "Clientes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioId",
                table: "Clientes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_RegiaoId",
                table: "Clientes",
                column: "RegiaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsuarioId",
                table: "Clientes",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Regioes_RegiaoId",
                table: "Clientes",
                column: "RegiaoId",
                principalTable: "Regioes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Usuarios_UsuarioId",
                table: "Clientes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
