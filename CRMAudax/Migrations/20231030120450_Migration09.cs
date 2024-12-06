using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration09 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataScore",
                table: "protestosDecisao");

            migrationBuilder.DropColumn(
                name: "QuantidadeProtestos",
                table: "protestosDecisao");

            migrationBuilder.RenameColumn(
                name: "ValorTotalProtestos",
                table: "protestosDecisao",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "UltimaOcorrenciaProtestos",
                table: "protestosDecisao",
                newName: "Uf");

            migrationBuilder.RenameColumn(
                name: "ProbabilidadeInadimplencia",
                table: "protestosDecisao",
                newName: "Natureza");

            migrationBuilder.RenameColumn(
                name: "Esclarecimento",
                table: "protestosDecisao",
                newName: "Moeda");

            migrationBuilder.AddColumn<string>(
                name: "Cartorio",
                table: "protestosDecisao",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "protestosDecisao",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<long>(
                name: "ClienteId",
                table: "protestosDecisao",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "protestosDecisao",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_protestosDecisao_ClienteId",
                table: "protestosDecisao",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_protestosDecisao_Clientes_ClienteId",
                table: "protestosDecisao",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_protestosDecisao_Clientes_ClienteId",
                table: "protestosDecisao");

            migrationBuilder.DropIndex(
                name: "IX_protestosDecisao_ClienteId",
                table: "protestosDecisao");

            migrationBuilder.DropColumn(
                name: "Cartorio",
                table: "protestosDecisao");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "protestosDecisao");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "protestosDecisao");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "protestosDecisao");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "protestosDecisao",
                newName: "ValorTotalProtestos");

            migrationBuilder.RenameColumn(
                name: "Uf",
                table: "protestosDecisao",
                newName: "UltimaOcorrenciaProtestos");

            migrationBuilder.RenameColumn(
                name: "Natureza",
                table: "protestosDecisao",
                newName: "ProbabilidadeInadimplencia");

            migrationBuilder.RenameColumn(
                name: "Moeda",
                table: "protestosDecisao",
                newName: "Esclarecimento");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataScore",
                table: "protestosDecisao",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "QuantidadeProtestos",
                table: "protestosDecisao",
                type: "bigint",
                nullable: true);
        }
    }
}
