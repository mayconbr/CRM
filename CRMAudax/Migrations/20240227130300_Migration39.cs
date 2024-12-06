using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration39 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArquivosPastas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    PastaId = table.Column<long>(type: "bigint", nullable: false),
                    PastasId = table.Column<long>(type: "bigint", nullable: true),
                    pathArquivo = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeArquivo = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tipoArquivo = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivosPastas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArquivosPastas_Pastas_PastasId",
                        column: x => x.PastasId,
                        principalTable: "Pastas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArquivosPastas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ArquivosPastas_PastasId",
                table: "ArquivosPastas",
                column: "PastasId");

            migrationBuilder.CreateIndex(
                name: "IX_ArquivosPastas_UsuarioId",
                table: "ArquivosPastas",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArquivosPastas");
        }
    }
}
