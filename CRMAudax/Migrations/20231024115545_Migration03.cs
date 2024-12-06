using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClasseRisco",
                table: "DecisaoScores",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "Esclarecimento",
                table: "DecisaoScores",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "ProbabilidadeInadimplencia",
                table: "DecisaoScores",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClasseRisco",
                table: "DecisaoScores");

            migrationBuilder.DropColumn(
                name: "Esclarecimento",
                table: "DecisaoScores");

            migrationBuilder.DropColumn(
                name: "ProbabilidadeInadimplencia",
                table: "DecisaoScores");
        }
    }
}
