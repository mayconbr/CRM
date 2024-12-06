using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration51 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArquivosDAP",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    pathArquivoDAP = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    nomeArquivoDAP = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    tipoArquivoDAP = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    dataEnvio = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivosDAP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArquivosDAP_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ArquivosDAP_ClienteId",
                table: "ArquivosDAP",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArquivosDAP");
        }
    }
}
