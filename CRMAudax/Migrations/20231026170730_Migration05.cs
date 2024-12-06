using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ValorTotalProtestos",
                table: "DecisaoScores",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "UltimaOcorrenciaProtestos",
                table: "DecisaoScores",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<long>(
                name: "QuantidadeProtestos",
                table: "DecisaoScores",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ValorTotalProtestos",
                table: "DecisaoScores",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DecisaoScores",
                keyColumn: "UltimaOcorrenciaProtestos",
                keyValue: null,
                column: "UltimaOcorrenciaProtestos",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UltimaOcorrenciaProtestos",
                table: "DecisaoScores",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AlterColumn<long>(
                name: "QuantidadeProtestos",
                table: "DecisaoScores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
