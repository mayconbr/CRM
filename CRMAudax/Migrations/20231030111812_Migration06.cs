using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration06 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "protestosDecisao",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Esclarecimento = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ProbabilidadeInadimplencia = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    DataScore = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    QuantidadeProtestos = table.Column<long>(type: "bigint", nullable: true),
                    UltimaOcorrenciaProtestos = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ValorTotalProtestos = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_protestosDecisao", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "protestosDecisao");
        }
    }
}
