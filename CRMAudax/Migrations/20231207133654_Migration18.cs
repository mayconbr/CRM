using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMAudax.Migrations
{
    /// <inheritdoc />
    public partial class Migration18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comarca",
                table: "pendenciasQuod",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "Forum",
                table: "pendenciasQuod",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "JusticeType",
                table: "pendenciasQuod",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "pendenciasQuod",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "ProcessAuthor",
                table: "pendenciasQuod",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "ProcessNumber",
                table: "pendenciasQuod",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "ProcessType",
                table: "pendenciasQuod",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.AddColumn<string>(
                name: "Vara",
                table: "pendenciasQuod",
                type: "longtext",
                nullable: true,
                collation: "latin1_swedish_ci")
                .Annotation("MySql:CharSet", "latin1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comarca",
                table: "pendenciasQuod");

            migrationBuilder.DropColumn(
                name: "Forum",
                table: "pendenciasQuod");

            migrationBuilder.DropColumn(
                name: "JusticeType",
                table: "pendenciasQuod");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "pendenciasQuod");

            migrationBuilder.DropColumn(
                name: "ProcessAuthor",
                table: "pendenciasQuod");

            migrationBuilder.DropColumn(
                name: "ProcessNumber",
                table: "pendenciasQuod");

            migrationBuilder.DropColumn(
                name: "ProcessType",
                table: "pendenciasQuod");

            migrationBuilder.DropColumn(
                name: "Vara",
                table: "pendenciasQuod");
        }
    }
}
