using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeituraNotificacaos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    NotificacaoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeituraNotificacaos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeituraNotificacaos_Notificacoes_NotificacaoId",
                        column: x => x.NotificacaoId,
                        principalTable: "Notificacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeituraNotificacaos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "StatusRotinas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    QtdCpf = table.Column<long>(type: "bigint", nullable: true),
                    QtdCnpj = table.Column<long>(type: "bigint", nullable: true),
                    QuodScorePJuridica = table.Column<long>(type: "bigint", nullable: true),
                    QuodScorePFisica = table.Column<long>(type: "bigint", nullable: true),
                    DecisaoScorePJuridica = table.Column<long>(type: "bigint", nullable: true),
                    DecisaoScorePFisica = table.Column<long>(type: "bigint", nullable: true),
                    DecisaoProtestosPJuridica = table.Column<long>(type: "bigint", nullable: true),
                    DecisaoProtestosPFisica = table.Column<long>(type: "bigint", nullable: true),
                    QuodProtestosPJuridica = table.Column<long>(type: "bigint", nullable: true),
                    QuodProtestosPFisica = table.Column<long>(type: "bigint", nullable: true),
                    DecisaoPendenciasPJuridica = table.Column<long>(type: "bigint", nullable: true),
                    DecisaoPendenciasPFisica = table.Column<long>(type: "bigint", nullable: true),
                    QuodPendenciasPJuiridica = table.Column<long>(type: "bigint", nullable: true),
                    QuodPendenciasPFisica = table.Column<long>(type: "bigint", nullable: true),
                    DataFinal = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusRotinas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateIndex(
                name: "IX_LeituraNotificacaos_NotificacaoId",
                table: "LeituraNotificacaos",
                column: "NotificacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_LeituraNotificacaos_UsuarioId",
                table: "LeituraNotificacaos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeituraNotificacaos");

            migrationBuilder.DropTable(
                name: "StatusRotinas");
        }
    }
}
