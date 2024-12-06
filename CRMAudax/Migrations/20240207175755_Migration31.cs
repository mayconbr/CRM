using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SituacaoCNPJ",
                table: "Clientes",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SituacaoCNPJ",
                table: "Clientes");
        }
    }
}
