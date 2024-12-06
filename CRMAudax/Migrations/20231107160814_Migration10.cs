using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Natureza",
                table: "protestosDecisao");

            migrationBuilder.AlterColumn<string>(
                name: "Score",
                table: "DecisaoScores",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.AddColumn<string>(
                name: "justificativaReprova",
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
                name: "justificativaReprova",
                table: "Clientes");

            migrationBuilder.AddColumn<string>(
                name: "Natureza",
                table: "protestosDecisao",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.UpdateData(
                table: "DecisaoScores",
                keyColumn: "Score",
                keyValue: null,
                column: "Score",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Score",
                table: "DecisaoScores",
                type: "longtext",
                nullable: false,
                collation: "latin1_swedish_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "latin1")
                .OldAnnotation("MySql:CharSet", "latin1")
                .OldAnnotation("Relational:Collation", "latin1_swedish_ci");
        }
    }
}
