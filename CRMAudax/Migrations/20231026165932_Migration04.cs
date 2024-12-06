using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "QuantidadeProtestos",
                table: "DecisaoScores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "UltimaOcorrenciaProtestos",
                table: "DecisaoScores",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValorTotalProtestos",
                table: "DecisaoScores",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeProtestos",
                table: "DecisaoScores");

            migrationBuilder.DropColumn(
                name: "UltimaOcorrenciaProtestos",
                table: "DecisaoScores");

            migrationBuilder.DropColumn(
                name: "ValorTotalProtestos",
                table: "DecisaoScores");
        }
    }
}
