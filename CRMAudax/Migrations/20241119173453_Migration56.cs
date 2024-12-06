using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration56 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogsErroRotina",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    Erro = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Consulta = table.Column<string>(type: "longtext", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    DataConsulta = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsErroRotina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsErroRotina_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateIndex(
                name: "IX_LogsErroRotina_ClienteId",
                table: "LogsErroRotina",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsErroRotina");
        }
    }
}
