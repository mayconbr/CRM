using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Parecer",
                table: "RelatoriosVisita",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Parecer",
                table: "RelatoriosVisita");
        }
    }
}
