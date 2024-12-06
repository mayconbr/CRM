using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration48 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TipoPessoaSocioTerceiro",
                table: "Clientes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TipoPessoaSocioSegundo",
                table: "Clientes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "TipoPessoaSocioTerceiro",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "TipoPessoaSocioSegundo",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
