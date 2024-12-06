using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "protestosQuod",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    NomeCartorio = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Endereco = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Cidade = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_protestosQuod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_protestosQuod_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateIndex(
                name: "IX_protestosQuod_ClienteId",
                table: "protestosQuod",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "protestosQuod");
        }
    }
}
