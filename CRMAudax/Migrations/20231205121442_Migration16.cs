using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pendenciasQuod",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    PendenciesControlCred = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CNPJ = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    CompanyName = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Nature = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ContractNumber = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    DateOcurred = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateInclued = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ParticipantType = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    ApontamentoStatus = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    City = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    State = table.Column<string>(type: "longtext", nullable: true, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pendenciasQuod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pendenciasQuod_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateIndex(
                name: "IX_pendenciasQuod_ClienteId",
                table: "pendenciasQuod",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pendenciasQuod");
        }
    }
}
