using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration57 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogsErroRotina_Clientes_ClienteId",
                table: "LogsErroRotina");

            migrationBuilder.DropIndex(
                name: "IX_LogsErroRotina_ClienteId",
                table: "LogsErroRotina");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "LogsErroRotina");

            migrationBuilder.AlterColumn<string>(
                name: "Erro",
                table: "LogsErroRotina",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataConsulta",
                table: "LogsErroRotina",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Consulta",
                table: "LogsErroRotina",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AddColumn<string>(
                name: "Documento",
                table: "LogsErroRotina",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Documento",
                table: "LogsErroRotina");

            migrationBuilder.UpdateData(
                table: "LogsErroRotina",
                keyColumn: "Erro",
                keyValue: null,
                column: "Erro",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Erro",
                table: "LogsErroRotina",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataConsulta",
                table: "LogsErroRotina",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "LogsErroRotina",
                keyColumn: "Consulta",
                keyValue: null,
                column: "Consulta",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Consulta",
                table: "LogsErroRotina",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AddColumn<long>(
                name: "ClienteId",
                table: "LogsErroRotina",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_LogsErroRotina_ClienteId",
                table: "LogsErroRotina",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogsErroRotina_Clientes_ClienteId",
                table: "LogsErroRotina",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
